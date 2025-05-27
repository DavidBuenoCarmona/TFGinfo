import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root"
})
export class ConfigurationService {
    private user: any = undefined;
    private role: number | undefined = undefined;
    private selectedUniversity: number | undefined = undefined;

    setUser(user: any): void {
        this.user = user;
    }

    getUser(): any {
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