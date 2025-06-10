import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackBarService } from './snackbar.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private snackBarService: SnackBarService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Excluir solicitudes a /assets o cualquier otra ruta pública
        if (req.url.includes('/assets')) {
            return next.handle(req);
        }
        // Obtén el token de autenticación desde localStorage
        const authToken = localStorage.getItem('token') ?? null;

        // Clona la solicitud y agrega la cabecera de autorización si el token está disponible
        const authReq = authToken
            ? req.clone({
                setHeaders: {
                    Authorization: `Bearer ${authToken}`
                }
            })
            : req;

        // Pasa la solicitud al siguiente manejador
        return next.handle(authReq).pipe(
            catchError((error: HttpErrorResponse) => {
                let message = error.error?.message || error.statusText || 'ERROR.GENERIC';
                this.snackBarService.error(message);
                return throwError(() => error);
            })
        );;
    }
}