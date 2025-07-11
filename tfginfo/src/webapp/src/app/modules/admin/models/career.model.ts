import { UniversityBase } from "./university.model";

export interface CareerBase {
    id?: number;
    name: string;
    doubleCareer: boolean;
}

export interface CareerDTO extends CareerBase {
    university?: UniversityBase;
    doubleCareers?: CareerDTO[];
}

export interface CareerFlatDTO extends CareerBase {
    universityId?: number;
    doubleCareers?: number[];
}