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
                           study.ProposedMap <- study.ProposedMap.Remove pId
                           study.AcceptedMap <- study.AcceptedMap.Add(pId, x)
                       | None   -> failwith "Project not in database!"
            | false ->
                study.ProposedMap <- study.ProposedMap.Remove pId

    let proposeProject (p : Project) (study : Study) =
        study.ProposedMap <- study.ProposedMap.Add(generateId(), p)

    let checkLimits (p : Project) : bool =
        match p.Groups with
        | Some gs -> gs.Length = (List.filter (fun (x : Group) -> 
                                    match p.limits.maxSize with
                                    | Some s -> x.members.Length <= s
                                    | None   -> true
                                ) gs).Length
        | None   -> true