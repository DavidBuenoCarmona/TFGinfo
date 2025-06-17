import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../../modules/login/services/auth.service';
import { ConfigurationService } from './configuration.service';
import { SnackBarService } from './snackbar.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private authService: AuthService,
        private router: Router,
        private configurationService: ConfigurationService,
        private snackBarService: SnackBarService) { }

    canActivate(): boolean {
        const token = localStorage.getItem('token');
        if (token && !this.configurationService.getUser()) {
            this.authService.checkToken(token).subscribe(user => {
                this.configurationService.setUser(user);
                this.configurationService.setRole(user.role.id);
                this.configurationService.setSelectedUniversity(user.universityId);
            });
            return true;
        } else if (!token) {
            // If no token is found, redirect to login
            this.snackBarService.show("ERROR.NOT_AUTHENTICATED");
            this.router.navigate(['/login']);
            return false;
        } else {
            // If user is already authenticated, allow access
            return true;
        }
    }
}