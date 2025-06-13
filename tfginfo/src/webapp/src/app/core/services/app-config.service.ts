import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class AppConfigService {
    private appConfig: any;
    private readonly http = inject(HttpClient);

    constructor() {

    }

    async loadAppConfig() {
        try {
            const data = await firstValueFrom(this.http.get('/assets/data/appConfig.json'));
            this.appConfig = data;
        } catch (error) {
            console.error("Error loading app config", error);
        }
    }

    getConfig() {
        return this.appConfig;
    }
}