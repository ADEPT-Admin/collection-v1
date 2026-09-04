import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, ElementRef, HostListener, forwardRef } from '@angular/core';
import { FormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { TooltipDirective } from 'src/app/Shared/Directives/tooltip.directive';
import { ShareDirectiveModule } from '../../Directives/share-directive.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { SafeHtmlPipe } from '../../Pipes/safe-html.pipe';
import { timer } from 'rxjs';

export interface SelectOption {
  label: string;
  value: any;
  text?: string;
  key?:string;
}

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    ShareDirectiveModule,
    TranslateModule,
    SafeHtmlPipe
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true
    }
  ]
})
export class SelectComponent implements ControlValueAccessor {
  @Input() options: SelectOption[] = [];
  @Input() placeholder: string = 'Select...';
  @Input() disabled = false;
  @Input() showfilter = true;
  @Input() allowNull = true;
  @Input() readonly = false;
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() icon: string = null;
  @Input() invalid: boolean = false;

  @Output() valueChange = new EventEmitter<any>();

  isOpen = false;
  selected: SelectOption | null = null;
  openUpwards = false;


  searchText = '';
  filteredOptions: SelectOption[] = [];
  value: any;
  onChange: any = () => {};
  onTouched: any = () => {};
  constructor(private el: ElementRef,private translate: TranslateService,) {
        this.translate.onLangChange.subscribe(item => {
        });
  }


  ngOnChanges() {
    if (this.options && this.value != null) {
      this.selected =
        this.options.find(o => o.value === this.value) || null;
    }

    timer(100).subscribe(() => {
      if(this.options){
      this.options.forEach(item => {
        if (!item.text && item.key) {
          item.label = this.translate.instant(item.key);
        }
      })
      }
    })
    if (this.isOpen) {
      this.filteredOptions = [...this.options];
    }
  }


writeValue(value: any): void {
  this.value = value;
  // ป้องกัน options ยังไม่ถูก set
  if (this.options && this.options.length > 0) {
    this.selected = this.options.find(o => o.value === value) || null;
  } else {
    this.selected = null;
  }
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

  toggleDropdown() {
    if (this.disabled) return;

    if (!this.isOpen) {
      // filter initial options
      this.filteredOptions = [...this.options];
      // auto flip
      const rect = this.el.nativeElement.getBoundingClientRect();
      const optionHeight = 36;
      const dropdownHeight = this.filteredOptions.length * optionHeight;
      const spaceBelow = window.innerHeight - rect.bottom;
      const spaceAbove = rect.top;
      this.openUpwards = spaceBelow < dropdownHeight && spaceAbove > dropdownHeight;
    }

    this.isOpen = !this.isOpen;
  }


  selectOption(option: SelectOption) {
    if (this.selected?.value === option.value) {
      if(this.allowNull){
        this.selected = null;
        this.value = null;
      }else{
        this.selected = option;
        this.value = option.value;
      }

    } else {
      this.selected = option;
      this.value = option.value;
    }

    this.onChange(this.value);
    this.onTouched();
    this.valueChange.emit(this.value);

    this.isOpen = false;
    this.searchText = '';
    this.filteredOptions = [];
  }

  search(event: Event) {
    const target = event.target as HTMLInputElement;
    this.searchText = target.value.toLowerCase();
    this.filteredOptions = this.options.filter(opt =>
      opt.label.toLowerCase().includes(this.searchText)
    );
  }


@HostListener('document:mousedown', ['$event'])
  clickOutside(event: Event) {
    if (!this.el.nativeElement.contains(event.target as Node)) {
      this.isOpen = false;
      this.searchText = '';
    }
  }

  @HostListener('search', ['$event'])
  onNativeSearch(event: Event) {
    event.stopPropagation();
  }

}
