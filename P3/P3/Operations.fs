namespace P3

open P3.Types

module Operations =

    let generateId () = (System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                                     System.Globalization.CultureInfo.InvariantCulture)
                        ).GetHashCode()

    let proposeProject (p : Project) (m : Map<(string * int), Project>) : Map<(string * int), Project> =
        m.Add((p.projectName, generateId()), p)
