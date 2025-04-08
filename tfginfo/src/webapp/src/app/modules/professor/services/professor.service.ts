import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { ProfessorDTO, ProfessorFlatDTO } from "../models/professor.model";

@Injectable({
    providedIn: 'root'
})
export class ProfessorService extends BaseService {
  
    constructor(
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) {
        super(http, appConfigService);
    }

    getProfessors(): Observable<ProfessorDTO[]> {
        return this.get(`${this.url}/professor`);
    }

    deleteProfessor(id: number): Observable<any> {
        return this.delete(`${this.url}/professor/${id}`);
    }

    getProfessor(id: number): Observable<ProfessorDTO> {
        return this.get(`${this.url}/professor/${id}`);
    }

    createProfessor(professor: ProfessorDTO): Observable<ProfessorFlatDTO> {
        return this.post(`${this.url}/professor`, professor);
    }

    updateProfessor(professor: ProfessorDTO): Observable<ProfessorFlatDTO> {
        return this.put(`${this.url}/professor`, professor);
    }
}