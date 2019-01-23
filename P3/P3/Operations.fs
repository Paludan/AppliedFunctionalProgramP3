namespace P3

open P3.Types

module Operations =    
    let generateId () = (System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                                     System.Globalization.CultureInfo.InvariantCulture)
                        ).GetHashCode()

    let handleProposedProject pId (study : Study) judgement =
            match judgement with
            | true  -> match study.ProposedMap.TryFind pId with
                       | Some x -> 
                           { study with
                                ProposedMap = study.ProposedMap.Remove pId
                                AcceptedMap = study.AcceptedMap.Add(pId, x) }
                       | None   -> failwith "Project not in database!"
            | false -> { study with ProposedMap = study.ProposedMap.Remove pId }

    let proposeProject (p : Project) (study : Study) =
        { study with ProposedMap = study.ProposedMap.Add(generateId(), p) }

    let reviewGroups (study : Study) = 
        let groupList = Map.fold (fun acc _ g -> 
                                let projNames =
                                    List.foldBack (fun pid acc -> 
                                                    match study.AcceptedMap.TryFind pid with 
                                                    | Some x -> x.projectName :: acc
                                                    | None   -> failwith "Project not found in Database!"
                                                ) g.projectPriorities []
                                let students = 
                                    List.foldBack (fun (m : Student) acc -> 
                                                    m.name :: acc
                                                ) g.members []
                                (g.GID, students, projNames) :: acc
                        ) [] study.Groups
        List.iter (fun (id,ml,pl) -> 
                printfn "Group %i: \n\tMembers: %s \n\tProjects: %s"id (String.concat ", " ml) (String.concat ", " pl)
            ) groupList
        |> ignore
        
    let checkLimits (p : Project) : bool =
        match p.Groups with
        | Some gs -> gs.Length = (List.filter (fun (x : Group) -> 
                                    match p.limits.maxSize with
                                    | Some s -> x.members.Length <= s
                                    | None   -> true
                                ) gs).Length
        | None   -> true

    let addGroup (g : Group) (pId : int) (study : Study) = 
        match study.AcceptedMap.TryFind pId with
        | Some p -> match p.Groups with 
                    | Some l -> { study with 
                                    AcceptedMap = study.AcceptedMap.Add(pId, { p with Groups = Some (g::l) })
                                } 
                    | None   -> { study with 
                                    AcceptedMap = study.AcceptedMap.Add(pId, { p with Groups = Some [g] })
                                }
        | None -> failwith "Project not in Database!"

    let lookupGroupPriorities group study =
        let projList = List.map (fun pid -> study.AcceptedMap.TryFind pid) group.projectPriorities
        let projNameList = List.map (
                                fun pOpt -> 
                                match pOpt with
                                | Some(p) -> p.projectName
                                | None -> "Project not assigned as an accepted project"
                           ) projList
        
        printfn "Group %i:" group.GID
        printfn "\tPriorities: %s" (String.concat ", " projNameList)
        |> ignore

    let unassignCurrentProject group study = 
        // find previously assigned projects if any
        let prevAssignedProj = Map.fold (fun acc k (p : Project) -> 
                                        match p.Groups with
                                        | Some(gList) when (List.exists (fun g -> g.GID = group.GID) gList) -> (k,p) :: acc
                                        | _ ->  acc
                                      ) [] study.AcceptedMap
        // remove the group form previous project
        let prevAssignedProj = List.map (fun (k,(p : Project)) -> 
                                            match p.Groups with 
                                            | Some(gList) -> 
                                                let gList' = List.filter (fun g -> g.GID <> group.GID) gList
                                                let gOpt' = if (List.isEmpty gList') then None else Some(gList')
                                                (k,{p with Groups = gOpt'})
                                        ) prevAssignedProj
        prevAssignedProj


    let assignToNewProj group projID study =
        let newProj = match study.AcceptedMap.TryFind projID with 
                      | Some(proj) -> proj
                      | None -> failwith "Project does not exists"
        // add group to new project
        let newProj' = match newProj.Groups with 
                       | Some(gList) -> 
                            let gList' = group :: gList 
                            let gOpt' = if (List.isEmpty gList') then None else Some(gList')
                            {newProj with Groups = gOpt'}
                       | None -> {newProj with Groups = Some([group])}
        newProj'

    let assignGroup group projID study = 
        let prevAssignedProj = unassignCurrentProject group study
        
        let newProj = assignToNewProj group projID study

        let newAcceptedMap = List.fold (fun mapPrev list -> 
                                Map.remove (fst list) mapPrev 
                                |> Map.add (fst list) (snd list)
                             ) study.AcceptedMap prevAssignedProj
                             |> Map.remove projID 
                             |> Map.add projID newProj
        // return study with changed assignment
        let tmpStudy = {study with AcceptedMap = newAcceptedMap}
        tmpStudy
