import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom, inject, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClient, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { routes } from './app.routes';
import { ConfigService } from './Shared/Services/config.service';
import { catchError, lastValueFrom, of, tap } from 'rxjs';
import { environment } from '../environments/environment';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService } from './Shared/Services/authentication/auth.service';
import { ShareService } from './Shared/Services/share.service';
import { Utils } from './Shared/Utilites/utils';
import { AppLoaderService } from './Shared/Components/loader/loader.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HttpAuthInterceptor } from './Shared/Interceptors/http-interceptor';
import { CustomTranslateLoader } from './Shared/Utilites/custom-translator-load';
import { PermissionService } from './Shared/Services/permission.service';
import { WorkService } from './Pages/work-list/work.service';
import localeEn from '@angular/common/locales/en';
import localeTh from '@angular/common/locales/th';
import { registerLocaleData } from '@angular/common';


registerLocaleData(localeEn);
registerLocaleData(localeTh);

export function HttpLoaderFactory(http: HttpClient, configService: ConfigService): TranslateLoader {
  return new CustomTranslateLoader(http, configService);
}

// ---------------- APP_INITIALIZER ----------------
export function initAppFactory() {
  const settingsService = inject(ConfigService);
  const http = inject(HttpClient);

  return () =>
    new Promise<void>((resolve) => {
      if (environment.production) {
        http.get('./config.json')
          .pipe(
            tap((data: any) => {
              settingsService.apiRoot = data.baseUrl;
              settingsService.setting = data.setting;
              resolve();  // ✅ resolve AFTER config is loaded
            }),
            catchError((error) => {
              console.error('Failed to load config.json, using fallback', error);
              settingsService.apiRoot = 'http://212.80.213.116:5111/adept';
              resolve();  // ✅ still resolve, but only here
              return of(null);
            })
          )
          .subscribe();
      } else {
        const settings = require('./config.json');
        settingsService.apiRoot = settings.baseUrl;
        settingsService.setting = settings.setting;
        resolve();  // ✅ resolve only after local settings assigned
      }
    });
}


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes) ,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpAuthInterceptor,
      multi: true
    },
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    {
      provide: APP_INITIALIZER,
      useFactory: initAppFactory,
      multi: true
    },

    importProvidersFrom(
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient, ConfigService]
        }
      })
    ),
    TranslateService,
    AuthService,
    ShareService,
    Utils,
    AppLoaderService,
    PermissionService,
    WorkService,
    provideAnimationsAsync(),

  ],

};
