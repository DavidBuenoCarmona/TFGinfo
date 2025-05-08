export interface WorkingGroupBase {
    id?: number;
    name: string;
    description: string;
    isPrivate: boolean;
}

export interface WorkingGroupProfessor {
    working_group: WorkingGroupBase;
    professor: number;
}

export interface WorkingGroupMessage {
    working_group: number;
    professor: number;
    message: string;
}