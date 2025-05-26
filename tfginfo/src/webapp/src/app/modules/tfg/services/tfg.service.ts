import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { TFGDTO, TFGLineDTO, TFGLineFlatDTO, TFGRequest, TFGRequestDTO } from "../models/tfg.model";
import { Filter } from "../../../core/core.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class TfgService extends BaseService {
  
    constructor
    (
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) 
    {
        super(http, appConfigService);
    }

    getTfgs(): Observable<TFGLineDTO[]> {
        return this.get(`${this.url}/tfg-line`);
    }

    searchTfgs(filters: Filter[]): Observable<TFGLineDTO[]> {
        return this.post(`${this.url}/tfg-line/search`, filters);
    }

    deleteTfg(id: number): Observable<any> {
        return this.delete(`${this.url}/tfg-line/${id}`);
    }

    getTfg(id: number): Observable<TFGLineDTO> {
        return this.get(`${this.url}/tfg-line/${id}`);
    }

    createTfg(tfg: TFGLineDTO): Observable<TFGLineFlatDTO> {
        return this.post(`${this.url}/tfg-line`, tfg);
    }

    updateTfg(tfg: TFGLineDTO): Observable<TFGLineFlatDTO> {
        return this.put(`${this.url}/tfg-line`, tfg);
    }

    addCareersToTfg(id: number, careers: number[]): Observable<TFGLineFlatDTO> {
        return this.post(`${this.url}/tfg-line/add-career/${id}`, careers);
    }

    addProfessorsToTfg(id: number, professors: number[]): Observable<TFGLineFlatDTO> {
        return this.post(`${this.url}/tfg-line/add-professor/${id}`, professors);
    }

    requestTfg(tfg: TFGRequest): Observable<void> {
        return this.post(`${this.url}/tfg/request`, tfg);
    }

    getTfgsByStudent(id: number): Observable<TFGLineDTO[]> {
        return this.get(`${this.url}/tfg-line/student/${id}`);
    }

    getTfgsByProfessor(id: number): Observable<TFGLineDTO[]> {
        return this.get(`${this.url}/tfg-line/professor/${id}`);
    }

    getPendingRequestsByProfessor(id: number): Observable<TFGRequestDTO[]> {
        return this.get(`${this.url}/tfg/professor-pending/${id}`);
    }

    acceptRequest(id: number): Observable<void> {
        return this.post(`${this.url}/tfg/accept/${id}`, null);
    }

    rejectRequest(id: number): Observable<void> {
        return this.post(`${this.url}/tfg/reject/${id}`, null);
    }

}