import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { of } from 'rxjs/internal/observable/of';

import { Observable } from 'rxjs/internal/Observable';
import { ConfigService } from '../config.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ShareService } from '../share.service';
import { SweetAlertService } from '../sweet-alert/sweet-alert.service';

@Injectable()
export class AuthService {
  constructor(
    private http: HttpClient,
    private shareService: ShareService,
    private configService : ConfigService,
    private _Router : Router,
    private sweetAlert : SweetAlertService,
    private translate : TranslateService
  ){



  }

  AuthenticateloginUser(DataLoginJson: string) {
    const url = this.configService.apiRoot + this.configService.loginUrl;
    return this.http.post(url, DataLoginJson )
      .pipe(tap(data => {
        return of(data);
      })).pipe(catchError(err => {
        return of(err);
      }));
  }

  getUserInfo(){
    const url = this.configService.apiRoot + this.configService.userInfoUrl;
    // const bodys = {
    //   "userName": userName
    // }

    return this.http.get(url)
      .pipe(tap(data => {
        return of(data);
      })).pipe(catchError(err => {
        return of(err);
      }));

  }



  changePassword(DataLoginJson: string) {
    const url = this.configService.apiRoot + this.configService.changePassUrl;
    return this.http.post(url, DataLoginJson)
      .pipe(tap(data => {
        return of(data);
      })).pipe(catchError(err => {
        return of(err);
      }));
  }

  changePasswordNewUser(DataLoginJson){
    const url = this.configService.apiRoot + this.configService.changePassNewUserUrl;
    return this.http.post(url, DataLoginJson)
      .pipe(tap(data => {
        return of(data);
      })).pipe(catchError(err => {
        return of(err);
      }));
  }


  refreshToken(): Observable<any> {

    const url = this.configService.apiRoot + this.configService.refreshTokenUrl;
    const payload = {accessToken : sessionStorage.getItem('token'),refreshToken : sessionStorage.getItem('re')};

    return this.http.post<any>(url, payload).pipe(
      map(res => {
        return { accessToken : res.data.accessToken , refreshToken : res.data.refreshToken };
      })
    );
  }





  isLoggedIn(): boolean {
    return this.shareService.getToken() != null && this.shareService.getToken() !== '' && localStorage.getItem('Reset') != '1';
  }

  // checkSessionTimeout(): Observable<boolean|object> {
  //   const headers = new HttpHeaders({ Authorization: this.shareService.getToken() ?? '', 'Content-Type': 'application/json' });
  //   const url = this.configService.apiRoot + this.configService.CheckSession;

  //   return this.http.post(url, {"projectID":this.shareService.getProjectId()}, { headers })
  //     .pipe(tap(resp => {
  //       return of(resp);
  //     }))
  //     .pipe(catchError(async err => {

  //       if(err.status == 0){
  //         const msg = await this.translate.get('CanNotConnectServer').toPromise();
  //         this.sweetAlert.showErrorMessage({html : msg})
  //       }

  //       console.error('Error Response checkSessionTimeout', err);

  //       return of(false);
  //     }));
  // }

  getAccessToken(): string|null {
    return this.shareService.getToken() != undefined ? this.shareService.getToken() : null;
  }


  logout(){

    const url = this.configService.apiRoot + this.configService.logOutUrl;
    return this.http.post(url, {})
      .pipe(tap(data => {
        return of(data);
      })).pipe(catchError(err => {
        return of(err);
      }));

  }
}
