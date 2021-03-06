namespace P3

module Types =
    type Student = {
        name: string
        email: string
    }

    and Group = {
       GID: int
       members: Student list
       projectPriorities: int list
    }

    and Teacher = {
       name: string
       email: string
       department: string
    }

    and Project = {
       projectName: string
       description: string
       supervisor: Teacher
       coSupervisor: Teacher list option
       Groups: Group list option
       limits: Limitations
    }

    and Limitations = {
       prereq: string list option
       maxSize: int option  
       maxGroups: int option
    }

    and HeadOfStudies = {
       teacher: Teacher
    }

    and Study = {
       AcceptedMap: Map<int, Project> // Projects accepted (PID -> Project)
       ProposedMap: Map<int, Project> // Projects proposed (PID -> Project)
       StudentMap:  Map<int, Student> // Student database (SID -> Student)
       Groups:      Map<int, Group>   // Groups (GID -> Group)
       Head:        HeadOfStudies
    }