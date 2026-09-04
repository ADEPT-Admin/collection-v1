import { Directive, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Directive({
  selector: '[appDebounceClick]',
  standalone : false
})
export class DebounceClickDirective {
  @Input() debounceTime = 500; // default 500ms
  @Output() onClick = new EventEmitter<Event>();

  private clicks = new Subject<Event>();

  constructor() {
    this.clicks
      .pipe(debounceTime(this.debounceTime))
      .subscribe(e => this.onClick.emit(e));
  }

  @HostListener('click', ['$event'])
  clickEvent(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    this.clicks.next(event);
  }
}
