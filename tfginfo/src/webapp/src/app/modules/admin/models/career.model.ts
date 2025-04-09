export interface CareerBase {
    id?: number;
    name: string;
}

export interface CareerDTO extends CareerBase {
    university: UniversityBase;
}

export interface CareerFlatDTO extends CareerBase {
    universityId: number;
}

export interface UniversityBase {
    id: number;
    name: string;
}