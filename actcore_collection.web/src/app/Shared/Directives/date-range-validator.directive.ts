import { Directive, Input, Optional, HostListener } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors, ControlContainer } from '@angular/forms';

@Directive({
  selector: '[appDateRangeInputValidator]',
  standalone : false,
  providers: [
    { provide: NG_VALIDATORS, useExisting: DateRangeInputValidatorDirective, multi: true }
  ]
})
export class DateRangeInputValidatorDirective implements Validator {

  @Input('appDateRangeInputValidator') pairName!: string;
  @Input() validatorTarget!: 'start' | 'end'; // 👈 extra input to control which field shows error

  constructor(
    @Optional() private controlContainer: ControlContainer
  ) {}

  validate(control: AbstractControl): ValidationErrors | null {
    if (!this.pairName || !this.controlContainer) return null;

    const formGroup = this.controlContainer.control;
    if (!formGroup) return null;

    const otherControl = formGroup.get(this.pairName);
    const currentValue = control.value;
    const otherValue = otherControl?.value;
    // Both empty — valid
    if (!currentValue && !otherValue){
      return null;
    }

    if (currentValue && !otherValue) {
      if(otherControl){
          otherControl.setErrors({requird : true})
      }
      return null;
    }


    if (!currentValue && otherValue) {
      return {requird : true};
    }

    return null;
  }
}
