import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { TFGLineDTO, TFGLineFlatDTO } from "../models/tfg.model";

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
}