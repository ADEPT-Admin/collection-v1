import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { InputNumberComponent } from '../input-number/input-number.component';
import { DateTimePickerComponent, DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { Utils } from '../../Utilites/utils';
import { ShareDirectiveModule } from '../../Directives/share-directive.module';
import { Subscription, timer } from 'rxjs';
import { CalendarComponent } from '../calendar/calendar.component';

@Component({
  selector: 'app-filter-select',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SelectComponent,
    TranslateModule,
    InputNumberComponent,
    DateTimePickerModule,
    ShareDirectiveModule,
    CalendarComponent
  ],
  templateUrl: './filter-select.component.html',
  styleUrls: ['./filter-select.component.scss']
})
export class FilterSelectComponent implements OnChanges {
  @ViewChild('myForm') form: NgForm;
  @ViewChild('startDateObj') startDateObj!: DateTimePickerComponent;
  @ViewChild('endDateObj') endDateObj!: DateTimePickerComponent;
  @Input() criteria: any[] = [];
  @Input() selectFilter: string = '';
  @Input() searchVal: any | null = null;
  @Input() inputType: string = 'text';
  @Output() selectFilterChange = new EventEmitter<string>();
  @Output() searchValChange = new EventEmitter<string>();
  @Output() inputTypeChange = new EventEmitter<string>();
  @Output() search = new EventEmitter<any>();
  public startDate: Date | null = null;
  public endDate: Date | null = null;
  public selectedOperator: string = '=';
  public selectedEnumValue: string;
  public minEndDate: Date | null = null;
  public maxStartDate: Date | null = null;
  public maskPlaceholderValue: Object = {day: 'DD', month: 'MM', year: 'YYYY', hour: 'HH', minute: 'MM', second: 'SS'}
  private allOperators = [
  { text: '=', value: '=' },
  { text: '>', value: '>' },
  { text: '<', value: '<' },
  { text: '>=', value: '>=' },
  { text: '<=', value: '<=' },
  { text: 'between', value: 'between' },
];
  operatorList: any = [];
  eNumList: any = [];
  $lang : Subscription = null;

  constructor(private utils: Utils, private translate: TranslateService,private cd: ChangeDetectorRef) {
    this.$lang = this.translate.onLangChange.subscribe(selectLang => {
        if (this.inputType.toLowerCase() === 'enum'){
          this.eNumList.map(c => ({
                label: c.label[this.translate.currentLang],
                value: c.value,
                key: c.label['en']
          }));
        }
    })

  }

  ngOnChanges() {

    if (this.criteria) {
      this.criteria.map(item => {
        item.label = item.key;
        item.value = item.key;
        return item
      })
    }
    if (this.selectFilter == '' && this.criteria) {
      this.selectFilter = this.criteria[0].value;
    }
  }

  setFilter(selectedKey: any) {
    this.selectFilter = selectedKey;
    this.selectFilterChange.emit(this.selectFilter);
    const selected = this.criteria.find(c => c.value === selectedKey);
    const type = selected?.type || 'text';
    this.inputType = type;
    this.inputTypeChange.emit(this.inputType);
    this.searchVal = '';
    this.startDate = null;
    this.endDate = null;
    this.selectedOperator = '=';
    this.minEndDate = null;
    this.maxStartDate = null;

  if (this.inputType.toLowerCase() === 'datetime' || this.inputType.toLowerCase() === 'date') {
    this.operatorList = this.allOperators.map(c => ({ label: c.text, value: c.value }));
  } else if (this.inputType.toLowerCase() === 'integer') {
    this.operatorList = this.allOperators
      .filter(c => c.value !== 'between')
      .map(c => ({ label: c.text, value: c.value }));
  } else if (this.inputType.toLowerCase() === 'enum') {

    this.eNumList = selected.values.map(c => ({
      label: c.label[this.translate.currentLang],
      value: c.value,
      key: c.label['en']
    }));
  }else {
    this.operatorList = this.allOperators
      .filter(c => ['=', '>', '<', '>=', '<='].includes(c.value))
      .map(c => ({ label: c.text, value: c.value }));
  }
  this.selectedOperator = this.operatorList.length > 0 ? this.operatorList[0].value : '=';
  }

  setFilteroperator(setFilteroperator: string) {
    this.selectedOperator = setFilteroperator;
    if (this.selectedOperator.toLowerCase() === 'between') {
      this.startDate = null;
      this.endDate = null;
    }
    this.minEndDate = null;
    this.maxStartDate = null;
  }

  changeEnumValue(enumValue: string) {
    this.selectedEnumValue = enumValue;
  }

  onSearchValChange(value: string) {
    this.searchVal = value;
    this.searchValChange.emit(this.searchVal);
  }

  applySearch() {
    this.form.control.markAllAsTouched();
    if (!this.form.valid) return;
    if (!this.selectFilter) return;
    const type = this.capitalizeType(this.inputType);
    const key = this.selectFilter;
    let operator = this.selectedOperator || null;
    let value: any = this.searchVal?.trim(), values: any = '';
    const toDateString = (dateInput: any) => {
    if (!dateInput) return '';
    const d = new Date(dateInput);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
  };
    if (type === 'Integer' || type === 'Decimal') {value = Number(value);}
    else if (type === 'Enum') value = this.selectedEnumValue;
    else if (type === 'Date') {
      operator = operator || '=';
      const { startDate: s, endDate: e } = this;
      if (operator.toLowerCase() === 'between') {
        ['startDate', 'endDate'].forEach(f => this.form.controls[f].markAsTouched());
        if ((s && !e) || (!s && e)) return Object.values(this.form.controls).forEach(c => c.markAsTouched());
        if (!s && !e) return this.search.emit({ selectFilter: key });
        values = { from: toDateString(s), to: toDateString(e) };
      } else {
        value = s ? toDateString(s) : '';
      }
    }
    else if (type === 'DateTime') {
      operator = operator || '=';
      const { startDate: s, endDate: e } = this;
      if (operator.toLowerCase() === 'between') {
        ['startDate', 'endDate'].forEach(f => this.form.controls[f].markAsTouched());
        if ((s && !e) || (!s && e)) return Object.values(this.form.controls).forEach(c => c.markAsTouched());
        if (!s && !e) return this.search.emit({ selectFilter: key });
        values = { from: this.utils.formatDateToUTCString(s), to: this.utils.formatDateToUTCString(e) };
      } else value = s ? this.utils.formatDateToUTCString(s) : '';
    }

    const payload = { operator, type, ...(values ? { values } : { value }) };
    this.search.emit(
      (value || value === 0 || values || type === 'Enum')
        ? { selectFilter: key, [key]: payload }
        : { selectFilter: key }
    );
  }


  private capitalizeType(type: string): string {
    const t = type?.toLowerCase();
    if (t === 'text' || t === 'string') return 'String';
    if (t === 'integer') return 'Integer';
    if (t === 'datetime') return 'DateTime';
    if (t === 'enum') return 'Enum';
    if (t === 'decimal') return 'Decimal';
    if (t === 'date') return 'Date';
    return 'String';
  }

  onStartDateChange(value: Date | null) {
    this.startDate = value;
    this.minEndDate = value ? new Date(value) : null;
  }

  onEndDateChange(value: Date | null) {
    this.endDate = value;
    this.maxStartDate = value ? new Date(value) : null;
  }

  ngOnDestroy(){
    this.$lang.unsubscribe();
  }

  clearedDate(event , field) {

      if(field == 'startDate'){
        this.form.controls['startDate']?.reset();
      }

      this.form.controls['startDate'].markAsPristine();
      this.form.controls['startDate'].markAsUntouched();
      this.form.controls['startDate'].updateValueAndValidity();

      if(field == 'endDate'){
        this.form.controls['endDate']?.reset();
      }

      this.form.controls['endDate'].markAsPristine();
      this.form.controls['endDate'].markAsUntouched();
      this.form.controls['endDate'].updateValueAndValidity();

  }

  numberOnly(event: any): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

}
















