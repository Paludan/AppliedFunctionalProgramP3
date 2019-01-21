namespace P3

module Types =
    type Student = {
        name: string
        ID: int
        study: string
    }

    and Group = {
        ID: int
        members: Student list
        projectPriorities: Project list
    }

    and Teacher = {
        name: string
        department: string
    }

    and Project = {
        title: string
        description: string
        supervisor: Teacher
        coSupervisor: Teacher list option
        limits: Limitations
    }

    and Limitations = {
        prereq: string list option
        maxSize: int option
        maxGroups: int option
    }

    and ProjectDatabase = {
        study: string
        projectMap: Map<int, Project>
    }

    and HeadOfStudies = {
        name: string
        study: string
    }
