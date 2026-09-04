import { Directive } from '@angular/core';
import {
  NG_ASYNC_VALIDATORS,
  AsyncValidator,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { ApiService } from '../Services/api.service';
import { ConfigService } from '../Services/config.service';
import { TranslateService } from '@ngx-translate/core';

@Directive({
  selector: '[appPasswordCheck]',
  standalone :false,
  providers: [
    { provide: NG_ASYNC_VALIDATORS, useExisting: PasswordCheckDirective, multi: true },
  ],
})
export class PasswordCheckDirective implements AsyncValidator {
  private debounceTimer: any;
  private lastValue: string = '';

  constructor(private api: ApiService, private config: ConfigService , private translate: TranslateService) {}

  validate(control: AbstractControl): Promise<ValidationErrors | null> {
    if (!control.value) return Promise.resolve(null);

    return new Promise((resolve) => {
      clearTimeout(this.debounceTimer);

      this.debounceTimer = setTimeout(async () => {
        // ป้องกันยิงซ้ำค่าเดิม
        if (this.lastValue === control.value) {
          resolve(null);
          return;
        }

        this.lastValue = control.value;

        try {
          const url = this.config.validatePasswordPolicyUrl;

          if(control.value == ''){
            resolve(null);
          }else{
            const res = await this.api.post<{ status: boolean; message?: string }>(url, {
              newPwd: control.value,
            });


            if (res.status) {
              resolve(null);
            } else {
              resolve({ invalidPassword: res.message || this.translate.instant('Password invalid') });
            }

          }



        } catch (e) {
          resolve({ serverError: this.translate.instant('Server error, please try again') });

        }
      }, 1000); // debounce 1500ms
    });
  }
}
