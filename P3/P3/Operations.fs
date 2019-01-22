namespace P3

open P3.Types

module Operations =    
    let generateId () = (System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                                     System.Globalization.CultureInfo.InvariantCulture)
                        ).GetHashCode()


    let proposeProject (p : Project) (study : Study) =
        study.ProposedMap <- study.ProposedMap.Add(generateId(), p)