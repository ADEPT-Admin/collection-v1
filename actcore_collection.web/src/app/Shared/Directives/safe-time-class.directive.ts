import { Directive, Input, ElementRef, Renderer2, OnInit } from '@angular/core';
import { DateTimePicker } from '@syncfusion/ej2-angular-calendars';

@Directive({
  selector: '[appSafeTimeClass]',
  standalone : false
})
export class SafeTimeClassDirective implements OnInit {
  @Input('appSafeTimeClass') picker!: DateTimePicker;
  @Input() disabledClass: string = 'disabled-timepicker';

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngOnInit() {
    if (!this.picker) return;

    // Initial check
    this.updateClass();

    // Subscribe to change event
    this.picker.change = (args: any) => {
      // Original change handler (if any)
      if (typeof this.picker.change === 'function') {
        this.picker.change(args);
      }
      this.updateClass();
    };

    // Subscribe to cleared event
    this.picker.cleared = (args: any) => {
      if (typeof this.picker.cleared === 'function') {
        this.picker.cleared(args);
      }
      this.updateClass();
    };
  }

  private updateClass() {
    const wrapper = this.picker.element.parentElement; // main wrapper
    if (!wrapper) return;

    if (this.picker.value) {
      this.renderer.removeClass(wrapper, this.disabledClass);
    } else {
      this.renderer.addClass(wrapper, this.disabledClass);
    }
  }
}
