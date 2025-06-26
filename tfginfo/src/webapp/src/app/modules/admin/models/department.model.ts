import { UniversityBase } from "./university.model";

export interface DepartmentBase
{
    id?: number;
    name: string;
    acronym: string;
}

export interface DepartmentDTO extends DepartmentBase
{
    universities?: UniversityBase[];
}

export interface DepartmentFlatDTO extends DepartmentBase
{
    universitiesId: number[];
}
