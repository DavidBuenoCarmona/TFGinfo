import { Injectable } from "@angular/core";
import { AppUserDTO } from "../../modules/admin/models/user.model";

@Injectable({
    providedIn: "root"
})
export class ConfigurationService {
    private user: AppUserDTO | undefined = undefined;
    private role: number | undefined = undefined;
    private selectedUniversites: number[] | undefined = undefined;

    setUser(user: AppUserDTO | undefined): void {
        this.user = user;
    }

    getUser(): AppUserDTO | undefined {
        return this.user;
    }

    setRole(role: number): void {
        this.role = role;
    }

    getRole(): number | undefined {
        return this.role;
    }

    setSelectedUniversities(universitiesId: number[] | undefined): void {
        this.selectedUniversites = universitiesId;
    }

    getSelectedUniversities(): number[] | undefined {
        return this.selectedUniversites;
    }
}