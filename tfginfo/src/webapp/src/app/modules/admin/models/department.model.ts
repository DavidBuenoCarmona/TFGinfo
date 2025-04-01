import { UniversityBase } from "./university.model";

export interface DepartmentBase
{
    id?: number;
    name: string;
}

export interface DepartmentDTO extends DepartmentBase
{
    university?: UniversityBase;
}

export interface DepartmentFlatDTO extends DepartmentBase
{
    universityId: number;
}
