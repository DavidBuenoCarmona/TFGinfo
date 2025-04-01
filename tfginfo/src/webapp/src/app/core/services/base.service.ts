import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { AppConfigService } from "./app-config.service";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root' // Esto hace que el servicio esté disponible en toda la aplicación
  })
export class BaseService {
    protected url!: string;
    constructor
    (
        protected http: HttpClient,
        protected appConfigService: AppConfigService
    ) 
    {
        this.url = this.appConfigService.getConfig().api.url;
    }

    get(url: string): Observable<any> {
        return this.http.get<any>(url);
    }

    post(url: string, body: any): Observable<any> {
        return this.http.post<any>(url, body);
    }

    put(url: string, body: any): Observable<any> {
        return this.http.put<any>(url, body);
    }

    delete(url: string): Observable<any> {
        return this.http.delete<any>(url);
    }
}