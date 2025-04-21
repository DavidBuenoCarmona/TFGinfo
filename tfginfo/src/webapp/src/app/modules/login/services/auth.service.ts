import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { BaseService } from "../../../core/services/base.service";
import { AppConfigService } from "../../../core/services/app-config.service";
import { Injectable } from "@angular/core";
import { AppUserDTO, NewUserDTO, UserBase, UserChangePassword, UserCredentials, UserDTO } from "../../admin/models/user.model";

@Injectable({
    providedIn: 'root' // Esto permite que Angular maneje la inyección globalmente
})
export class AuthService extends BaseService {
  
    constructor
    (
        protected override http: HttpClient,
        protected override appConfigService: AppConfigService
    ) 
    {
        super(http, appConfigService);
    }

    login(credentials: UserCredentials): Observable<NewUserDTO> {
        return this.post(`${this.url}/auth/login`, credentials);
    }

    changePassword(changePasswordRequest: UserChangePassword): Observable<boolean> {
        return this.post(`${this.url}/auth/change-password`, changePasswordRequest);
    }
}