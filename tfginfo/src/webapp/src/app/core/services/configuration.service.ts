import { Injectable } from "@angular/core";
import { AppUserDTO } from "../../modules/admin/models/user.model";

@Injectable({
    providedIn: "root"
})
export class ConfigurationService {
    private user: AppUserDTO | undefined = undefined;
    private role: number | undefined = undefined;
    private selectedUniversity: number | undefined = undefined;

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

    setSelectedUniversity(universityId: number | undefined): void {
        this.selectedUniversity = universityId;
    }

    getSelectedUniversity(): number | undefined {
        return this.selectedUniversity;
    }
}