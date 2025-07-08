import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { DepartmentDTO, DepartmentFlatDTO } from "../models/department.model";
import { Filter, ImportResponse } from "../../../core/core.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class DepartmentService extends BaseService {
  
    constructor
    (
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) 
    {
        super(http, appConfigService);
    }

    getDepartments(): Observable<DepartmentDTO[]> {
        return this.get(`${this.url}/department`);
    }

    deleteDepartment(id: number): Observable<any> {
        return this.delete(`${this.url}/department/${id}`);
    }

    getDepartment(id: number): Observable<DepartmentDTO> {
        return this.get(`${this.url}/department/${id}`);
    }

    createDepartment(department: DepartmentDTO): Observable<DepartmentFlatDTO> {
        return this.post(`${this.url}/department`, department);
    }

    updateDepartment(department: DepartmentDTO): Observable<DepartmentFlatDTO> {
        return this.put(`${this.url}/department`, department);
    }

    searchDepartments(filters: Filter[]): Observable<DepartmentDTO[]> {
        return this.post(`${this.url}/department/search`, filters);
    }

    importFromCSV(content: string): Observable<ImportResponse> {
        return this.post(`${this.url}/department/import`, { content });
    }
}