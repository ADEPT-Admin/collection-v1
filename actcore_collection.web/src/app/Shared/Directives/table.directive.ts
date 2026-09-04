import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  Renderer2,
} from '@angular/core';
import { timer } from 'rxjs';

@Directive({
    selector: '[appTable]',
    standalone: false
})
export class TableDirective {
  constructor(private el: ElementRef, private _renderer: Renderer2) {
    this.setup();
  }

  toggle(button: HTMLElement) {
    const body = this.el.nativeElement.querySelector('tbody');
    const show = !body.classList.contains('hide');
    if (show) {
      body.classList.add('hide');
      button.classList.remove('fa-chevron-down');
      button.classList.add('fa-chevron-right');
    } else {
      body.classList.remove('hide');
      button.classList.remove('fa-chevron-right');
      button.classList.add('fa-chevron-down');
    }
  }

  setup() {
    timer(10).subscribe(() => {
      const toggleBtn = this.el.nativeElement.querySelector('.toggleTable');
      toggleBtn.addEventListener('click', (event) => {
        this.toggle(toggleBtn);
      });
    });
  }
}
