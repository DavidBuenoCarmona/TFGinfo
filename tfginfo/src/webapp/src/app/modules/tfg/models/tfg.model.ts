import { CareerDTO } from "../../admin/models/career.model";
import { DepartmentDTO } from "../../admin/models/department.model";
import { ProfessorDTO } from "../../professor/models/professor.model";

export interface TFGLineBase {
    id?: number;
    name: string;
    description: string;
    slots: number;
    group: boolean;
}


export interface TFGLineDTO extends TFGLineBase {
    department?: DepartmentDTO;
    careers?: CareerDTO[];
    professors?: ProfessorDTO[];
}


export interface TFGLineFlatDTO extends TFGLineBase {
    departmentId: number;
}


export interface TFGBase {
    id?: number;
    startDate?: Date;
    external_tutor_name?: string;
    external_tutor_email?: string;
    accepted: boolean;
}

export interface TFGDTO extends TFGBase {
    tfgLine?: TFGLineDTO;
}

export interface TFGFlatDTO extends TFGBase {
    tfgLineId: number;
}

export interface TFGRequest {
    studentEmail: string;
    professorId: number;
    secondaryProfessorId?: number;
    tfg: TFGFlatDTO;
}

export interface TFGRequestDTO {
    studentName: string;
    tfgName: string;
    tfgId: number;
}
