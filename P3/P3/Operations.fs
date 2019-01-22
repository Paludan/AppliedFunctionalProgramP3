namespace P3

open P3.Types

module Operations =    
    let generateId () = (System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                                     System.Globalization.CultureInfo.InvariantCulture)
                        ).GetHashCode()


    let proposeProject (p : Project) (study : Study) =
        study.ProposedMap <- study.ProposedMap.Add(generateId(), p)
    
    let reviewGroups (study : Study) = 
        let groupList = Map.fold (fun acc _ g -> List.append acc [(g.GID, List.fold (fun acc (m : Student) -> acc @ [m.name]) [] g.members, List.fold (fun acc p -> acc @ [p.projectName]) [] g.projectPriorities)] ) [] study.Groups
        List.iter (fun (id,ml,pl) -> printfn "Group %i: \n\tMembers: %s \n\tProjects: %s"id (String.concat ", " ml) (String.concat ", " pl) ) groupList
        |> ignore
        //let memberList = List.fold (fun acc ) [] groupList
    //    Map.fold (fun acc id g -> acc + "%s " (List.fold (fun acc stud -> acc + stud.sname) "" g.members) ) "" study.G