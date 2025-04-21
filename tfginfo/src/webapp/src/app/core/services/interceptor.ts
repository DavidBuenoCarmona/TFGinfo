import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Excluir solicitudes a /assets o cualquier otra ruta pública
    if (req.url.includes('/assets')) {
      return next.handle(req);
    }
    // Obtén el token de autenticación desde localStorage
    const user = localStorage.getItem('user');
    const authToken = user ? JSON.parse(user).token : null;

    // Clona la solicitud y agrega la cabecera de autorización si el token está disponible
    const authReq = authToken
      ? req.clone({
          setHeaders: {
            Authorization: `Bearer ${authToken}`
          }
        })
      : req;

    // Pasa la solicitud al siguiente manejador
    console.log('authReq :>> ', authReq);
    return next.handle(authReq);
  }
}