import { DepartmentBase } from "../../admin/models/department.model";

export class ProfessorBase {
    id?: number;
    name!: string;
    surname!: string;
    email!: string;
    departmentBoss!: boolean;

    constructor(model?: Partial<ProfessorBase>) {
        if (model) {
            this.id = model.id;
            this.name = model.name!;
            this.surname = model.surname!;
            this.email = model.email!;
            this.departmentBoss = model.departmentBoss!;
        }
    }
}

export class ProfessorDTO extends ProfessorBase {
    department!: DepartmentBase;
}

export class ProfessorFlatDTO extends ProfessorBase {
    departmentId!: number; // ID del departamento
}

export class NewProfessorDTO {
    professor!: ProfessorDTO;
    auth_code!: string; // Código de autorización
}