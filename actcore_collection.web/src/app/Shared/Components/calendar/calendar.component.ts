import {
  Component, forwardRef, HostListener,
  Input , ElementRef,
  Output,
  EventEmitter,
  ViewChild
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  Validator,
  AbstractControl,
  ValidationErrors,
  FormsModule
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';

import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';

@Component({
  selector: 'app-calendar',
  imports: [CommonModule , FormsModule , TranslateModule , NgxMaskDirective],
  standalone : true,
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CalendarComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CalendarComponent),
      multi: true
    },
    DatePipe,
    provideNgxMask()
  ]
})
export class CalendarComponent implements ControlValueAccessor , Validator {

  @ViewChild('wrapper', { static: true }) wrapper!: ElementRef;
  open = false;
  value: Date | null = null;

  year!: number;
  month!: number;
  day!: number;
  hour = 0;
  minute = 0;


  currentLang = '';
  years: number[] = [];




  @Input() minDate?: Date;
  @Input() maxDate?: Date;
  @Input() showToday = true;
  @Input() format = 'dd/MM/yyyy';
  @Input() showTime = true;
  @Input() placeholder = "SelectDate"

  @Input() readonly = false;
  @Input() disabled = false;
  @Input() enableMark = false;
  @Input() required = false;

  openUpwards = false;

  @Output() valueChange = new EventEmitter<Date>();
  @Output() cleared  = new EventEmitter();

  currentLocale = 'en-US';



  constructor(
    private translate: TranslateService ,
    private el: ElementRef,
    private datePipe: DatePipe
  ) {
    this.currentLang = this.translate.currentLang || 'th';

    this.setLocale(this.translate.currentLang);

    this.initYears();


    if(this.currentLang == 'th'){
      this.weekdays = this.thWeekdays;
    }else{
      this.weekdays = this.enWeekdays;
    }

    this.translate.onLangChange.subscribe(lang => {
      this.currentLang = lang.lang;

      this.setLocale(lang.lang);

      if(this.currentLang == 'th'){
        this.weekdays = this.thWeekdays;
      }else{
        this.weekdays = this.enWeekdays;
      }


    });
  }

  setLocale(lang: string) {
    this.currentLocale = lang === 'th' ? 'th-TH' : 'en-US';
  }

  validate(control: AbstractControl): ValidationErrors | null {

    let value: Date | null = control.value;
    if (!value) {
      if(!this.required){
        return null;
      }
      return { required: true };
    }

    if(typeof(value) == 'string'){
      value = new Date(value)
    }

    const current = new Date(
      value.getFullYear(),
      value.getMonth(),
      value.getDate()
    );

    if (this.minDate) {
      const min = new Date(
        this.minDate.getFullYear(),
        this.minDate.getMonth(),
        this.minDate.getDate()
      );

      if (current < min) {
        return { required: true };
      }
    }

    if (this.maxDate) {
      const max = new Date(
        this.maxDate.getFullYear(),
        this.maxDate.getMonth(),
        this.maxDate.getDate()
      );

      if (current > max) {
        return { required: true };
      }
    }

    return null;
  }




  // ✅ เริ่มสัปดาห์วันอาทิตย์
  weekdays = [];
  thWeekdays = ['อา', 'จ', 'อ', 'พ', 'พฤ', 'ศ', 'ส'];
  enWeekdays = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
  thaiShortMonths = [
    'ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.',
    'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.',
    'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.'
  ];

  enShortMonths = [
    'Jan', 'Feb', 'Mar', 'Apr',
    'May', 'Jun', 'Jul', 'Aug',
    'Sep', 'Oct', 'Nov', 'Dec'
  ];

  hours = Array.from({ length: 24 }, (_, i) => i);
  minutes = Array.from({ length: 60 }, (_, i) => i);

  onChange = (_: any) => {};
  onTouched = () => {};

  minYear?: number;
  maxYear?: number;


  // ===== ControlValueAccessor =====
  writeValue(val: Date | null): void {
    this.value = val ? new Date(val) : null;

    // ✅ ถ้า null → ไม่ต้อง set calendar
    if (!this.value) return;

    this.year = this.value.getFullYear();
    this.month = this.value.getMonth();
    this.day = this.value.getDate();
    this.hour = this.value.getHours();
    this.minute = this.value.getMinutes();

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


  toggle(event: MouseEvent) {
    event?.stopPropagation();
    if (this.disabled) return;
    if (this.readonly) return;


    // ถ้าเปิด calendar ครั้งแรก ค่อย set today
    if (!this.value && this.open === false) {
      const today = new Date();
      this.year = today.getFullYear();
      this.month = today.getMonth();
      this.day = today.getDate();
    }else{
      this.year = this.value.getFullYear();
      this.month = this.value.getMonth();
      this.day = this.value.getDate();
    }

    // คำนวณตำแหน่งก่อน set open = true
      const inputRect = this.wrapper.nativeElement.getBoundingClientRect();
      const popupHeight = 320;
      const spaceBelow = window.innerHeight - inputRect.bottom;
      const spaceAbove = inputRect.top;
      this.openUpwards = spaceBelow < popupHeight && spaceAbove > popupHeight;


    this.open = !this.open;

  }



  /** ✅ Sunday start → JS getDay() ใช้ตรงได้เลย */
  get firstDayOffset(): number {
    return new Date(this.year, this.month, 1).getDay(); // 0 = Sunday
  }

  get calendarDays(): (number | null)[] {
    const total = new Date(this.year, this.month + 1, 0).getDate();
    const blanks = Array(this.firstDayOffset).fill(null);
    const days = Array.from({ length: total }, (_, i) => i + 1);
    return [...blanks, ...days];
  }

  selectDate(d: number) {
    const date = new Date(this.year, this.month, d);
    if (this.enableTime && this.value) {
      date.setHours(this.value.getHours());
      date.setMinutes(this.value.getMinutes());
    }

    this.updateValue(date);
    this.day = d;
    this.open = false

  }


  updateValue(date: Date | null) {
    this.value = date;
    this.onChange(date);
    this.onTouched();
    this.valueChange.emit(this.value);
  }

  updateTime() {
    if (!this.value) {
      this.value = new Date();
    }

    this.value.setHours(this.hour);
    this.value.setMinutes(this.minute);

    this.updateValue(this.value);
  }


  prevMonth() {
    if (--this.month < 0) {
      this.month = 11;
      this.year--;
    }
  }

  nextMonth() {
    if (++this.month > 11) {
      this.month = 0;
      this.year++;
    }
  }

  isSelected(d: number) {
    return this.day === d;
  }

  initYears() {
    const current = this.year ?? new Date().getFullYear();
    const start = current - 120;
    const end = current + 99;

    this.years = Array.from(
      { length: end - start + 1 },
      (_, i) => start + i
    );

    this.minYear = start;
    this.maxYear = end;

    // default ปีปัจจุบันถ้า year ยังไม่มีค่า
    if (!this.year) {
      this.year = new Date().getFullYear(); // 2025
    }


    // const current = this.year ?? new Date().getFullYear();
    // this.years = Array.from(
    //   { length: 202 },
    //   (_, i) => current - 100 + i
    // );

    // this.minYear = this.years[0];
    // this.maxYear = this.years[this.years.length - 1];

  }

  get displayValue(): string {
    if (!this.value) return '';

    return this.value ? this.formatDate(this.value) : '';
  }

  formatDate(date: Date): string {


    if (!date) return '';

    const isTH = this.currentLang === 'th';
    const locale = isTH ? 'th-TH' : 'en-US';

    const year = date.getFullYear();
    const buddhistYear = year + 543;

    // 🔒 placeholder ที่ DatePipe จะไม่ parse แน่นอน
    const YEAR4 = '\uFFF0';
    const YEAR2 = '\uFFF1';

    const safeFormat = this.format
      .replace(/yyyy/g, YEAR4)
      .replace(/yy/g, YEAR2);

    let result = this.datePipe.transform(
      date,
      safeFormat,
      undefined,
      locale
    ) || '';

    result = result
      .replace(new RegExp(YEAR4, 'g'),
        isTH ? buddhistYear.toString() : year.toString()
      )
      .replace(new RegExp(YEAR2, 'g'),
        isTH ? buddhistYear.toString().slice(-2) : year.toString().slice(-2)
      );

    return result;
  }

  get monthOptions(): string[] {
    return this.currentLang === 'th' ? this.thaiShortMonths : this.enShortMonths;
  }


  get monthLabel(): string {
    if (this.currentLang === 'th') {
      return `${this.thaiShortMonths[this.month]} ${this.year + 543}`;
    }

    return new Intl.DateTimeFormat('en-US', {
      month: 'short',
      year: 'numeric'
    }).format(new Date(this.year, this.month, 1));
  }

  selectToday() {
    const today = new Date();
   if (!this.isTodayEnabled()) return;
    this.year = today.getFullYear();
    this.month = today.getMonth();
    this.day = today.getDate();
    this.updateValue(today);
  }

  isTodayEnabled(): boolean {
    const today = new Date(
      new Date().getFullYear(),
      new Date().getMonth(),
      new Date().getDate()
    );

    if (this.minDate) {
      const min = new Date(
        this.minDate.getFullYear(),
        this.minDate.getMonth(),
        this.minDate.getDate()
      );
      if (today < min) return false;
    }

    if (this.maxDate) {
      const max = new Date(
        this.maxDate.getFullYear(),
        this.maxDate.getMonth(),
        this.maxDate.getDate()
      );
      if (today > max) return false;
    }

    return true;
  }

  isDateSelectable(day: number): boolean {
    const date = this.buildDate(day);

    const current = new Date(
      date.getFullYear(),
      date.getMonth(),
      date.getDate()
    );

    if (this.minDate) {
      const min = new Date(
        this.minDate.getFullYear(),
        this.minDate.getMonth(),
        this.minDate.getDate()
      );
      if (current < min) return false;
    }

    if (this.maxDate) {
      const max = new Date(
        this.maxDate.getFullYear(),
        this.maxDate.getMonth(),
        this.maxDate.getDate()
      );
      if (current > max) return false;
    }

    return true;
  }


  canGoPrev(): boolean {
    if (!this.minDate) return true;
    const prev = new Date(this.year, this.month - 1, 1);
    return prev >= new Date(this.minDate.getFullYear(), this.minDate.getMonth(), 1);
  }

  canGoNext(): boolean {
    if (!this.maxDate) return true;
    const next = new Date(this.year, this.month + 1, 1);
    return next <= new Date(this.maxDate.getFullYear(), this.maxDate.getMonth(), 1);
  }


  private buildDate(day: number): Date {
    return new Date(this.year, this.month, day);
  }


  get enableTime(): boolean {
    return this.showTime && /H|m/.test(this.format);
  }


  pad(n: number) {
    return n.toString().padStart(2, '0');
  }


  clear(event?: MouseEvent) {
    event?.stopPropagation();
    this.value = null;

    this.onChange(null);   // แจ้ง Angular Forms
    this.onTouched();

    // reset calendar view เป็นวันนี้ (optional)
    const today = new Date();
    this.year = today.getFullYear();
    this.month = today.getMonth();
    this.open = false;
    this.hour = 0;
    this.minute = 0;
    this.updateValue(this.value)

    this.cleared.emit()
  }


  get dateMask(): string {
    // dd/MM/yyyy หรือ dd/MM/พ.ศ.
    return this.formatToMask(this.format);
  }


  private formatToMask(format: string): string {
    return format
      .replace('dd', 'd0')
      .replace('MM', 'M0')
      .replace('yyyy', '0000')
      .replace('HH', 'Hh')
      .replace('mm', 'm0')
      .replace('ss', 's0');
  }


  isFocused = false;

  onFocus() {
    this.isFocused = true;
  }

  onBlur(event: FocusEvent) {

    const input = event.target as HTMLInputElement;

    let raw = input.value
      .replace(/_/g, '')
      .trim();

    if (!raw) {
      this.clear();
      input.value = '';
      return;
    }

    const normalized = this.normalizeByFormat(raw, this.format);

    if (!normalized) {
      this.clear();
      input.value = '';
      return;
    }

    let parsed = this.parseByFormatLoose(
      normalized,
      this.format,
      this.currentLang === 'th'
    );

    if (!parsed) {
      this.clear();
      input.value = '';
      return;
    }


    this.updateValue(parsed);

    this.writeValue(parsed)

    // 🔁 แสดงกลับเป็น พ.ศ.
    input.value = this.formatDate(parsed);

  }

  normalizeByFormat(value: string, format: string): string | null {
    if (format === 'dd/MM/yyyy') {
      const parts = value.split('/');
      if (parts.length !== 3) return null;

      let [d, m, y] = parts;

      if (d.length === 1) d = '0' + d;
      if (m.length === 1) m = '0' + m;
      if (y.length !== 4) return null;

      return `${d}/${m}/${y}`;
    }

    return value;
  }

  parseByFormat(
    value: string,
    format: string,
    isThai: boolean
  ): Date | null {

    let d = 0, m = 0, y = 0, hh = 0, mm = 0, ss = 0;

    try {
      if (format === 'dd/MM/yyyy') {
        [d, m, y] = value.split('/').map(Number);
      }

      if (format === 'dd/MM/yyyy HH:mm') {
        const [date, time] = value.split(' ');
        [d, m, y] = date.split('/').map(Number);
        [hh, mm] = time.split(':').map(Number);
      }

      if (format === 'yyyy-MM-dd') {
        [y, m, d] = value.split('-').map(Number);
      }

      // 🇹🇭 พ.ศ. ➜ ค.ศ.
      if (isThai && y > 2400) {
        y -= 543;
      }

      const date = new Date(y, m - 1, d, hh, mm, ss);

      // validate date จริง
      if (
        date.getFullYear() !== y ||
        date.getMonth() !== m - 1 ||
        date.getDate() !== d
      ) {
        return null;
      }

      return date;
    } catch {
      return null;
    }
  }

  parseByFormatLoose(
    value: string,
    format: string,
    isThai: boolean
  ): Date | null {

    let d = 0, m = 0, y = 0, hh = 0, mm = 0, ss = 0;

    try {
      if (format === 'dd/MM/yyyy') {
        [d, m, y] = value.split('/').map(Number);
      }

      if (format === 'dd/MM/yyyy HH:mm') {
        const [date, time] = value.split(' ');
        [d, m, y] = date.split('/').map(Number);
        [hh, mm] = time.split(':').map(Number);
      }

      if (format === 'dd/MM/yyyy HH:mm:ss') {
        const [date, time] = value.split(' ');
        [d, m, y] = date.split('/').map(Number);
        [hh, mm , ss] = time.split(':').map(Number);
      }

      if (format === 'yyyy-MM-dd') {
        [y, m, d] = value.split('-').map(Number);
      }

      // 🇹🇭 พ.ศ. ➜ ค.ศ.
      if (isThai && y > 2400) {
        y -= 543;
      }

      if(y > this.years[this.years.length - 1]){
        y = this.years[this.years.length - 1];
      }

      if(y < this.years[0]){
        y = this.years[0];
      }


      // ❗ basic guard
      if (!y || m < 1 || m > 12 || d < 1) {
        return null;
      }

      // ✅ smart-correct day overflow
      const maxDay = new Date(y, m, 0).getDate();
      const correctedDay = Math.min(d, maxDay);

      const date = new Date(y, m - 1, correctedDay, hh, mm, ss);

      // validate month/year only
      if (
        date.getFullYear() !== y ||
        date.getMonth() !== m - 1
      ) {
        return null;
      }


      return date;
    } catch {
      return null;
    }
  }


  getLastDayOfMonth(year: number, month: number): number {
    // month = 1-12
    return new Date(year, month, 0).getDate();
  }

  correctDayOverflow(
    day: number,
    month: number,
    year: number
  ): number {
    const maxDay = this.getLastDayOfMonth(year, month);
    return Math.min(day, maxDay);
  }



  get maskPlaceholder(): string {
    // แสดงโครง format แทน ___
    return this.format;
  }



  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {

    if (!this.open) return;

    if (!this.el.nativeElement.contains(event.target)) {
      this.open = false;
    }
  }

  @HostListener('document:keydown.escape')
  onEsc() {
    this.open = false;
  }
}
