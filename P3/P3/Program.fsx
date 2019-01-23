#load "Types.fs"
#load "Operations.fs"

open P3.Types
open P3.Operations

let studies = Map.empty
let softtech = {
    Study.Head = {
        HeadOfStudies.teacher = {
            Teacher.name       = "Michael"
            Teacher.department = "Computer Science"
        }
    }
    Study.AcceptedMap = Map.empty
    Study.ProposedMap = Map.empty
    Study.StudentMap  = Map.empty   
    Study.Groups      = Map.empty 
}

let p = {
    Project.projectName = "TestProject";
    Project.description = "testDesc";
    Project.supervisor = {
                     name = "Supervisor";
                     department = "testDep";
                 };
    Project.coSupervisor = None;
    Project.limits = {Limitations.prereq = None;
        Limitations.maxSize = None;
        Limitations.maxGroups = None;};
    Project.Groups = None
}


proposeProject p softtech
let g = {Group.GID = 1; Group.members = []; projectPriorities = [-1665586549] }

studies.Add("Software Technology", softtech)

[<EntryPoint>]
let main argv =
    
    let tryf = (Map.find "Software Technology" studies)
    printfn "%A" (Map.find 1 tryf)
    //printfn "%A" a
    0 // return an integer exit code
