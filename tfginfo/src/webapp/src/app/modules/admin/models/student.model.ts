export interface StudentBase {
    id?: number;
    name: string;
    surname: string;
    email: string;
    dni: string;
    phone?: string;
    address?: string;
    birthdate?: Date;
}

export interface StudentDTO extends StudentBase {
    career: CareerBase;
}

export interface StudentFlatDTO extends StudentBase {
    careerId: number;
}

export interface CareerBase {
    id: number;
    name: string;
}