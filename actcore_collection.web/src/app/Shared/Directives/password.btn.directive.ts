import { AfterViewChecked, Directive, ElementRef, HostListener, Input, OnChanges, Renderer2 } from '@angular/core';
import { timer } from 'rxjs';

@Directive({
    selector: '[appPasswordBtn]',
    standalone: false
})
export class PasswordBtnDirective implements AfterViewChecked {

  private _shown = false;

  private isDisabled = false;
  private isInvalid = false;


  constructor(private el: ElementRef , private _renderer:Renderer2,) {
      this.setup();
  }

  toggle(span: HTMLElement) {
    this._shown = !this._shown;
    if (this._shown) {
      // this.el.nativeElement.setAttribute('type', 'text');
      // this.el.nativeElement.children[0].firstChild.setAttribute('type', 'text');
      this.el.nativeElement.setAttribute('type', 'text');

      span.innerHTML = 'visibility_off';
    } else {
      // this.el.nativeElement.setAttribute('type', 'password');
      // this.el.nativeElement.children[0].firstChild.setAttribute('type', 'password');
      this.el.nativeElement.setAttribute('type', 'password');
      span.innerHTML = 'visibility';
    }
  }


  ngAfterViewChecked(): void {
    // const isDisabled = this.el.nativeElement.children[0].classList.contains('e-disabled');
    const isDisabled = this.el.nativeElement.classList.contains('disabled');
    const isInvalid = this.el.nativeElement.classList.contains('is-invalid');
    const parent = this.el.nativeElement.parentNode;
    const span = parent.querySelector('mat-icon');

    if(isDisabled != this.isDisabled){
      this.isDisabled = isDisabled;

      if(!isDisabled){
        span.classList.remove('disabled')
      }else{
        span.classList.add('disabled')
      }

    }

     if(isInvalid != this.isInvalid){
      this.isInvalid = isInvalid;

      if(!isInvalid){
          span.classList.remove('text-danger')
      }else{
          span.classList.add('text-danger')
      }
    }

  }

  setup() {
    const parent = this.el.nativeElement.parentNode;
    const span = document.createElement('mat-icon');
    span.className = 'mat-icon cursor-pointer material-icons mat-icon-no-color';
    span.innerHTML = `visibility`;
    span.addEventListener('click', (event) => {
      if(!this.isDisabled){
        this.toggle(span);
      }
    });
    parent.classList.add('passwordWrap');
    parent.appendChild(span);
  }


}
