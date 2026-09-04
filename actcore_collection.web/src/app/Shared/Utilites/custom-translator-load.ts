import { ConfigService } from '../Services/config.service';  // ปรับ path ตาม project ของคุณ
import { TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export class CustomTranslateLoader implements TranslateLoader {
  constructor(
    private http: HttpClient,
    private configService: ConfigService,   // ใช้ ConfigService
    private localPrefix: string = '/assets/i18n/',
    private suffix: string = '.json'
  ) {}

  getTranslation(lang: string): Observable<any> {
    const url = `${this.configService.apiRoot}${this.configService.getLangUrl}${lang}`;
    return this.http.get<any>(url).pipe(
      map((res: any) => (res && res.data ? res.data : res)),
      catchError(() => this.http.get<any>(`${this.localPrefix}${lang}${this.suffix}`))
    );
  }
}
