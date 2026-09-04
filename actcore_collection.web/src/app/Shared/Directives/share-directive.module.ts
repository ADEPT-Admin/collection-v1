import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DecimalInputDirective } from './decimal.directive';
import { PasswordBtnDirective } from './password.btn.directive';
import { AutofillMonitorDirective } from './autofill.monitor.directive';
import { TableDirective } from './table.directive';
import { PasswordCheckDirective } from './password.validate.directive';
import { DebounceClickDirective } from './debounce-click.directive';
import { ConfirmPasswordValidatorDirective } from './password.confirm.directive';
import { TooltipDirective } from './tooltip.directive';
import { DateRangeInputValidatorDirective } from './date-range-validator.directive';
import { SafeTimeClassDirective } from './safe-time-class.directive';



@NgModule({
  declarations: [
    DecimalInputDirective,
    PasswordBtnDirective,
    AutofillMonitorDirective,
    TableDirective,
    PasswordCheckDirective,
    DebounceClickDirective,
    ConfirmPasswordValidatorDirective,
    TooltipDirective,
    DateRangeInputValidatorDirective,
    SafeTimeClassDirective
  ],
  imports: [
  ],
  exports : [
    DecimalInputDirective,
    PasswordBtnDirective,
    AutofillMonitorDirective,
    TableDirective,
    PasswordCheckDirective,
    DebounceClickDirective,
    ConfirmPasswordValidatorDirective,
    TooltipDirective,
    DateRangeInputValidatorDirective,
    SafeTimeClassDirective
  ]
})
export class ShareDirectiveModule { }
