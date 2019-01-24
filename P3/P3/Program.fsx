#load "Types.fs"
#load "Operations.fs"

open P3.Types
open P3.Operations

let studies = Map.empty

let p1 = {
    Project.projectName = "TestProject1";
    Project.description = "testDesc";
    Project.supervisor = {
                     name = "Supervisor1";
                     department = "Computer Science";
                     email = "123@123.com"
                 };
    Project.coSupervisor = None;
    Project.limits = {Limitations.prereq = None;
        Limitations.maxSize = Some(1);
        Limitations.maxGroups = Some(3);};
    Project.Groups = None
}

let p2 = {
    Project.projectName = "TestProject2";
    Project.description = "testDesc";
    Project.supervisor = {
                     name = "Supervisor2";
                     department = "Computer Science";
                     email = "321@321.com"
                 };
    Project.coSupervisor = None;
    Project.limits = {Limitations.prereq = None;
        Limitations.maxSize = Some(3);
        Limitations.maxGroups = Some(2);};
    Project.Groups = None
}

let p3 = {
    Project.projectName = "TestProject3";
    Project.description = "testDesc";
    Project.supervisor = {
                     name = "Supervisor2";
                     department = "Computer Science";
                     email = "321@321.com"
                 };
    Project.coSupervisor = None;
    Project.limits = {Limitations.prereq = None;
        Limitations.maxSize = Some(3);
        Limitations.maxGroups = Some(2);};
    Project.Groups = None
}


let s1 ={
        Student.name = "Student1"; 
        Student.email = "stud1@dtu.mail"
}

let s2 ={
        Student.name = "Student2"; 
        Student.email = "stud2@dtu.mail"
}

let s3 ={
        Student.name = "Student3"; 
        Student.email = "stud3@dtu.mail"
}

let s4 ={
        Student.name = "Student4"; 
        Student.email = "stud4@dtu.mail"
}

let g1 = {
    Group.GID = generateId(); 
    Group.members = [s1;s2]; 
    projectPriorities = [1;2] 
}

let g2 = {
    Group.GID = generateId(); 
    Group.members = [s3;s4]; 
    projectPriorities = [1] 
}

let softtech = {
    Study.Head = {
        HeadOfStudies.teacher = {
            Teacher.name       = "Michael"
            Teacher.department = "Computer Science"
            Teacher.email      = "michael@email.her"
        }
    }
    Study.AcceptedMap = Map.empty.Add(1,p1).Add(2,p2)
    Study.ProposedMap = Map.empty.Add(3, p3)
    Study.StudentMap  = Map.empty.Add(1,s1).Add(2,s2).Add(3,s3).Add(4,s4)
    Study.Groups      = Map.empty.Add(g1.GID,g1).Add(g2.GID,g2)
}

[<EntryPoint>]
let main argv =
    printfn "reviewGroups output: "
    let studytech' = handleProposedProject 3 softtech true // The updated studytech after handling a proposed project
    reviewGroups studytech'

    printfn "-------------------------------------------------------------"

    printfn "The whole of softtech study:"
    printfn "%A" (assignGroup g1 1 studytech')

    0 // return an integer exit code