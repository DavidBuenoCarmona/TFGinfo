import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../../modules/login/services/auth.service';
import { ConfigurationService } from './configuration.service';
import { SnackBarService } from './snackbar.service';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private authService: AuthService,
        private router: Router,
        private configurationService: ConfigurationService,
        private snackBarService: SnackBarService) { }

    canActivate(): Observable<boolean> {
        const token = localStorage.getItem('token');
        if (token && !this.configurationService.getUser()) {
            return this.authService.checkToken(token).pipe(
                map(user => {
                    this.configurationService.setUser(user);
                    this.configurationService.setRole(user.role.id);
                    this.configurationService.setSelectedUniversities(user.universitiesId);
                    return true;
                }),
                catchError(() => {
                    this.snackBarService.show("ERROR.NOT_AUTHENTICATED");
                    this.router.navigate(['/login']);
                    return of(false);
                })
            );
        } else if (!token) {
            this.snackBarService.show("ERROR.NOT_AUTHENTICATED");
            this.router.navigate(['/login']);
            return of(false);
        } else {
            return of(true);
        }
    }
}