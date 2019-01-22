namespace P3

module Types =
    type Student = {
        mutable name: string
    }

    and Group = {
        mutable GID: int
        mutable members: Student list
        mutable projectPriorities: int list
    }

    and Teacher = {
        mutable name: string
        mutable department: string
    }

    and Project = {
        mutable projectName: string
        mutable description: string
        mutable supervisor: Teacher
        mutable coSupervisor: Teacher list option
        mutable Groups: Group list option
        mutable limits: Limitations
    }

    and Limitations = {
        mutable prereq: string list option
        mutable maxSize: int option
        mutable maxGroups: int option
    }

    and ProjectDatabase = {
        mutable projectMap: Map<(int * string), Project>
    }

    and HeadOfStudies = {
        mutable teacher: Teacher
    }

    and Study = {
        mutable AcceptedMap: Map<int, Project> // Projects accepted (PID -> Project)
        mutable ProposedMap: Map<int, Project> // Projects proposed (PID -> Project)
        mutable StudentMap:  Map<int, Student> // Student database (SID -> Student)
        mutable Groups:      Map<int, Group>   // Groups (GID -> Group)
        mutable Head:        HeadOfStudies
    }