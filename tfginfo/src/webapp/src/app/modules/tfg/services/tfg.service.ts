import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";

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

    getTfgs(): Observable<any[]> {
        return this.get(`${this.url}/tfg-line`);
    }

    deleteTfg(id: number): Observable<any> {
        return this.delete(`${this.url}/tfg-line/${id}`);
    }
}