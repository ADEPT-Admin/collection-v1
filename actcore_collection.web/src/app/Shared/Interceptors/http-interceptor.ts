import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../Services/authentication/auth.service';
import { Utils } from '../Utilites/utils';
import { TranslateService } from '@ngx-translate/core';
import { ConfigService } from '../Services/config.service';
import { AppLoaderService } from '../Components/loader/loader.service';

@Injectable()
export class HttpAuthInterceptor implements HttpInterceptor {

  constructor(private router: Router , private autService : AuthService , private utils : Utils , private tranlsate : TranslateService , private configService : ConfigService , private loaderService : AppLoaderService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Clone request และเพิ่ม Authorization header
    const token = sessionStorage.getItem('token');
    let clonedReq = req;



    if (token) {
      clonedReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        }
      });
    }

    // ส่ง request และจัดการ error
    return next.handle(clonedReq).pipe(
      catchError((error: HttpErrorResponse) => {


        if (error.status === 404) {

          const showError = (async () => {
              this.loaderService.hideAll();
              const res = await this.utils.dialogMessageAll('error', `${this.tranlsate.instant('CanNotConnectServer')}`);
              if(res){
                return throwError(() => error);
              }


          })
          showError()
        }

        if (error.status === 401) {


          if(req.url.includes(this.configService.refreshTokenUrl) ){
            this.utils.dialogMessageAll('error', `${this.tranlsate.instant('Session Expired')}`);
            sessionStorage.clear();
            this.router.navigate(['/login']);
            return throwError(() => error);

          }


            // 🔄 Token expired → call refresh token endpoint
          return this.autService.refreshToken().pipe(
            switchMap((newToken: any | null) => {
              if (newToken) {
                // Retry original request with new token
                const retryReq = req.clone({
                  setHeaders: { Authorization: `Bearer ${newToken.accessToken}` }
                });
                sessionStorage.setItem('token' , newToken.accessToken)
                sessionStorage.setItem('re' , newToken.refreshToken)
                return next.handle(retryReq);
              } else {
                // Refresh failed → redirect to login

                this.utils.dialogMessageAll('error', `${this.tranlsate.instant('Session Expired')}`);
                sessionStorage.clear();
                this.router.navigate(['/login']);
                return throwError(() => error);
              }
            }),
            catchError(() => {
              // If refresh also fails → redirect to login
              this.utils.dialogMessageAll('error', `${this.tranlsate.instant('Session Expired')}`);
              sessionStorage.clear();
              this.router.navigate(['/login']);
              return throwError(() => error);
            })
          );
        }
        return throwError(() => error);

        // throw error;
      })
    );
  }
}
