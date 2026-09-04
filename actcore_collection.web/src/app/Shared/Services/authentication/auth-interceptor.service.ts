import { ConfigService } from './../config.service';
import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  constructor(
    private configService : ConfigService ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    try{
      if (req.url.startsWith(this.configService.apiRoot)){
        const token = sessionStorage.getItem('token') ?? null;
          const headers = new HttpHeaders()
            .set('Authorization', ` ${token}`)
            .set('Content-Type', 'application/json');
          const authReq = req.clone({headers});

          return next.handle(authReq).pipe(
              catchError(error => {
                if (error.status === 401 || error.status === 403) {
                  // handle error
                }
                return throwError(() => error);
              })
          )


      }else {
        return next.handle(req);
      }
    } catch (error) {
      return throwError(() => error);
    }

  }

}
