import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appConfirmPasswordValidator]',
  standalone : false,
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: ConfirmPasswordValidatorDirective,
      multi: true
    }
  ]
})
export class ConfirmPasswordValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors | null {
    if (!control.parent) return null;

    const password = control.parent.get('newPassword') || control.parent.get('password');
    const confirmPassword = control;

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordMismatch: true };
    }
    return null;
  }
}
