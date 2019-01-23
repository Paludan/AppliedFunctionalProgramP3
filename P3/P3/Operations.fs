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
                                    List.foldBack (fun p acc -> 
                                                    p.projectName :: acc
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
        | Some x -> match x.Groups with 
                    | Some l -> x.Groups <- Some (g::l)
                    | None   -> x.Groups <- Some [g]
        | None -> failwith ""
