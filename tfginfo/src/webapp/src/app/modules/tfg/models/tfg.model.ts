import { CareerDTO } from "../../admin/models/career.model";
import { DepartmentDTO } from "../../admin/models/department.model";

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
}


export interface TFGLineFlatDTO extends TFGLineBase {
    departmentId: number;
}