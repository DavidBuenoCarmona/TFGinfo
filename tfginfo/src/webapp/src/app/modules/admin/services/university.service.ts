import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { UniversityBase } from "../models/university.model";
import { Filter, ImportResponse } from "../../../core/core.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class UniversityService extends BaseService {
  
    constructor(
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) {
        super(http, appConfigService);
    }

    getUniversities(): Observable<UniversityBase[]> {
        return this.get(`${this.url}/university`);
    }

    deleteUniversity(id: number): Observable<any> {
        return this.delete(`${this.url}/university/${id}`);
    }

    getUniversity(id: number): Observable<UniversityBase> {
        return this.get(`${this.url}/university/${id}`);
    }

    createUniversity(university: UniversityBase): Observable<UniversityBase> {
        return this.post(`${this.url}/university`, university);
    }

    updateUniversity(university: UniversityBase): Observable<UniversityBase> {
        return this.put(`${this.url}/university`, university);
    }

    searchUniversities(filter: Filter[]): Observable<UniversityBase[]> {
        return this.post(`${this.url}/university/search`, filter);
    }

    importFromCSV(content: string): Observable<ImportResponse> {
        return this.post(`${this.url}/university/import`, { content });
    }
}