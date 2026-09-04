import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { SafeHtmlPipe } from '../../Pipes/safe-html.pipe';

@Component({
  selector: 'app-input-with-button',
  templateUrl: './input-with-button.component.html',
  styleUrls: ['./input-with-button.component.scss'],
  imports : [SafeHtmlPipe],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputWithButtonComponent),
      multi: true
    }
  ]
})
export class InputWithButtonComponent implements ControlValueAccessor {
  @Input() placeholder: string = 'Enter text';
  @Input() icon: string = 'pi pi-search';
  @Output() buttonClick = new EventEmitter<void>();
  @Output() clearClick = new EventEmitter<void>();
  @Input() name: string = '';
  @Input() id: string = '';
  @Input() displayHtml :boolean = false;
  @Input() invalid: boolean = false;
  value: string = '';
  disabled = false;

  onChange: any = () => {};
  onTouched: any = () => {};

  writeValue(value: any): void {
    this.value = value || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInputChange(value: string) {
    this.value = value;
    this.onChange(this.value);
    this.onTouched();
  }

  onButtonClick() {
    this.buttonClick.emit();
  }


  onClearClick(){
    this.clearClick.emit();

  }
}
