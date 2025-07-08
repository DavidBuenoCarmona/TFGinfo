import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { CareerDTO, CareerFlatDTO } from "../models/career.model";
import { Filter, ImportResponse } from "../../../core/core.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class CareerService extends BaseService {
  
    constructor
    (
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) 
    {
        super(http, appConfigService);
    }

    getCareers(): Observable<CareerDTO[]> {
        return this.get(`${this.url}/career`);
    }

    deleteCareer(id: number): Observable<any> {
        return this.delete(`${this.url}/career/${id}`);
    }

    getCareer(id: number): Observable<CareerDTO> {
        return this.get(`${this.url}/career/${id}`);
    }

    createCareer(career: CareerDTO): Observable<CareerFlatDTO> {
        return this.post(`${this.url}/career`, career);
    }

    updateCareer(career: CareerDTO): Observable<CareerFlatDTO> {
        return this.put(`${this.url}/career`, career);
    }

    searchCarrers(filters: Filter[]): Observable<CareerDTO[]> {
        return this.post(`${this.url}/career/search`, filters);
    }

    importFromCSV(content: string): Observable<ImportResponse> {
        return this.post(`${this.url}/career/import`, {content});
    }
}