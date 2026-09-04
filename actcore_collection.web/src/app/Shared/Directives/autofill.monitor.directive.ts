import { Directive, ElementRef, HostListener, Input, NgZone, Renderer2 ,OnInit  } from '@angular/core';
import { timer } from 'rxjs';


@Directive({
    selector: '[appAutofillMonitor]',
    standalone: false
})
export class AutofillMonitorDirective {
  private elRef: HTMLInputElement;
  constructor( private ngZone: NgZone , private _renderer:Renderer2) {

  }


  @HostListener('animationstart', ['$event'])
  onAnimationStart(event: AnimationEvent): void {
    if (event.animationName === 'onAutoFillStart') {
      // Autofill detected, handle it
      this.handleAutofill();
    }
  }

  private handleAutofill(): void {
    // Access the input value or perform any desired action
    // const autofillValue = this.elementRef.nativeElement.value;
    // You can emit an event or perform any action based on the autofill detection
  }



  ngOnInit(): void {
    timer(10).subscribe(() => {
        // this.elRef = this.elementRef.nativeElement.children[0].children[0];
        // this.elRef = this.elementRef;

        if (this.elRef) {
        this.detectAutofill().then((isAutofilled) => {
          if(isAutofilled){
            timer(1000).subscribe(() =>{
              // document.click()
            }
            )
          }
        // this.nativeAutofill.emit(isAutofilled);
        });
        }
    })

  }


  private detectAutofill(): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      setTimeout(() => {
        const isAutofilled =
        window.getComputedStyle(this.elRef, null).getPropertyValue("appearance") === "menulist-button";
        resolve(isAutofilled);
      }, 1200);
    });
  }



}
