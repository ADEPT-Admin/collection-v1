import { Component, Input, forwardRef, ElementRef, ViewChild, Output, EventEmitter } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ShareDirectiveModule } from '../../Directives/share-directive.module';

@Component({
  selector: 'app-input-number',
  standalone: true,
  imports: [CommonModule , ShareDirectiveModule],
  templateUrl: './input-number.component.html',
  styleUrls: ['./input-number.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputNumberComponent),
      multi: true
    },
    DecimalPipe
  ]
})
export class InputNumberComponent implements ControlValueAccessor {
  @Input() name: string = '';
  @Input() id: string = '';
  @Input() min: number = Number.MIN_SAFE_INTEGER;
  @Input() max: number = Number.MAX_SAFE_INTEGER;
  @Input() step: number = 1;
  @Input() showButtons: boolean = false;
  @Input() fractionDigits: number = null;
  @Input() readonly: boolean = false;
  @Input() invalid: boolean = false;
  @ViewChild('inputEl') inputEl!: ElementRef<HTMLInputElement>;
  value: number = 0;
  rawValue: string = '';
  disabled = false;

  private formatTimeout: any = null;
  private onChange = (value: any) => {};
  private onTouched = () => {};

  // ControlValueAccessor
  // writeValue(val: number | null): void {
  //   if (val === null || val === undefined) {
  //     this.value = 0; // default value
  //   } else {
  //     this.value = val;
  //   }

  //   // Format using fractionDigits
  //   this.rawValue = this.formatValue(this.value);

  //   if (this.inputEl) {
  //     this.inputEl.nativeElement.value = this.rawValue;
  //   }

  // }

  writeValue(val: number | string | null | undefined): void {
    let normalized: number;

    // 👇 normalize ค่า
    if (val === '' || val === null || val === undefined) {
      normalized = 0;
    } else {
      normalized = Number(val);
    }

    // กัน NaN
    if (isNaN(normalized)) {
      normalized = 0;
    }

    this.value = normalized;
    this.rawValue = this.formatValue(this.value);

    if (this.inputEl) {
      this.inputEl.nativeElement.value = this.rawValue;
    }

    // 🔥 สำคัญ: sync model เมื่อ incoming เป็น empty
    if (val === '' || val === null || val === undefined) {
      this.onChange(this.value);
    }
  }


  registerOnChange(fn: any): void { this.onChange = fn; }
  registerOnTouched(fn: any): void { this.onTouched = fn; }
  setDisabledState?(isDisabled: boolean): void { this.disabled = isDisabled; }

  // Keypress/input handler with formatting

  onInput(event: any) {
  if (this.readonly || this.disabled) return;

  const input = event.target as HTMLInputElement;
  let cursorPos = input.selectionStart || 0;
  const prevLength = input.value.length;

  let numericStr = input.value.replace(/,/g, '');


  const dotIndex = input.value.indexOf('.');

  // Allow one minus at start if min < 0
  if (numericStr.startsWith('-') && this.min >= 0) {
    numericStr = numericStr.slice(1);
  }

  const x = numericStr.split(".");

  // Only allow digits + 1 decimal + optional minus
  if (!/^(-?\d*(\.\d*)?)?$/.test(numericStr)) {
    input.value = this.rawValue; // revert invalid
    return;
  }

  // Parse number
  const numeric = parseFloat(numericStr);
  if (!isNaN(numeric)) {
    this.value = numeric;
    this.onChange(this.value);
  } else {
    this.value = 0;
    this.onChange(this.value);
  }



  // -------------------------
  // CASE 1: cursor อยู่ก่อนจุดทศนิยม → format ทันที
  // -------------------------
  if (cursorPos < dotIndex || dotIndex === -1) {
    this.rawValue = this.formatValue(this.value);
    input.value = this.rawValue;

    const newLength = this.rawValue.length;
    cursorPos = cursorPos + (newLength - prevLength);
    input.setSelectionRange(cursorPos, cursorPos);

    return;
  }

  // -------------------------
  // CASE 2: cursor อยู่หลังจุดทศนิยม → debounce format
  // -------------------------

  clearTimeout(this.formatTimeout);

  this.formatTimeout = setTimeout(() => {
    // Format only after delay
    this.rawValue = this.formatValue(this.value);
    input.value = this.rawValue;

    // reposition cursor at the end (best behavior for decimals)
    const end = input.value.length;
    input.setSelectionRange(end, end);

  }, 1200); // = 1000ms delay (ปรับได้)
}

  onBlur(event: FocusEvent) {
    const input = event.target as HTMLInputElement;
    const numericStr = input.value.replace(/,/g, '');
    const numeric = Number(numericStr);

    this.value = isNaN(numeric) ? 0 : numeric;

    this.rawValue = this.formatValue(this.value);
    input.value = this.rawValue;

    this.onChange(this.value);
    this.onTouched();
  }

  increment() {
    this.value = this.clampValue(this.value + this.step);
    this.rawValue = this.formatValue(this.value);
    this.onChange(this.value);
  }

  decrement() {
    this.value = this.clampValue(this.value - this.step);
    this.rawValue = this.formatValue(this.value);
    this.onChange(this.value);
  }

  private clampValue(val: number): number {
    if (val < this.min) val = this.min;
    if (val > this.max) val = this.max;
    return parseFloat(val.toFixed(this.fractionDigits));
  }

  private formatValue(val: number): string {
    const digits = this.fractionDigits ?? 0; // use 0 if null or undefined

    const floored = this.floorDecimal(val, digits);


    return floored.toLocaleString('en-US', {
      minimumFractionDigits: digits,
      maximumFractionDigits: digits
    });
  }


  private floorDecimal(value: number, digits: number): number {
    const factor = Math.pow(10, digits);

    // กรณีติดลบต้องใช้ Math.ceil แทน floor
    return value >= 0
      ? Math.floor(value * factor) / factor
      : Math.ceil(value * factor) / factor;
  }

}
