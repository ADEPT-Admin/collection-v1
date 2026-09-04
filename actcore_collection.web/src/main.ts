import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

import { registerLicense } from '@syncfusion/ej2-base';
import { environment } from './environments/environment';
registerLicense(environment.syncfusionLisenceKey);

import { registerLocaleData } from '@angular/common';
import localeTh from '@angular/common/locales/th';
import localeThExtra from '@angular/common/locales/extra/th';

registerLocaleData(localeTh, 'th-TH');


bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
