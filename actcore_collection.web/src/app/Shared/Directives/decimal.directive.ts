import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
    selector: '[appDecimalInput]',
    standalone: false
})
export class DecimalInputDirective {
  private regex: RegExp = new RegExp(/^-?\d*\.?\d*$/);
  @Input('appDecimalInput') type: string = '';
  constructor(private el: ElementRef) {}

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    let str = this.el.nativeElement.value;
    const key = ['v','c','a','x','z'];

    if(this.el.nativeElement.nodeName == "EJS-TEXTBOX"){
        const input = this.el.nativeElement.querySelector('input.e-textbox');
        str = input.value;
    }
    const index = (event.target as any).selectionStart;
    const inputValue: string = str.slice(0, index) + event.key + str?.slice(index);

    const allowedKeys: string[] = [
      'Backspace',
      '-',
      '.',
      'ArrowLeft',
      'ArrowRight',
      'Delete',
      'Tab',
      'Control',
      'v',
      'c',
      'a',
      'x',
      'z'
    ];
    const countMinus = this.countMinusFromStart(inputValue);
    const countDot = this.countDot(inputValue);
    const checkDot = this.checkDotFromStart(inputValue ,event.key, index);
    if(event.code == "Space"){
      event.preventDefault();
    }


    if (!allowedKeys.includes(event.key) &&(isNaN(Number(event.key)) || (event.key === '-' && inputValue.indexOf('-') > -1) || (event.key === '.' && inputValue.indexOf('.') > -1))){
      event.preventDefault();
    }else{

      if(key.includes(event.key.toLowerCase()) &&  !event.ctrlKey){
        event.preventDefault();
      }else{

        if(event.key == '.'){

          if(this.type == 'int' || this.type == 'uint'){
            if(countDot > 0){
              event.preventDefault();
            }
          }else if(this.type == 'float'){
            if(countDot > 1){
              event.preventDefault();
            }
            if(checkDot == true){
              event.preventDefault();
            }
          }



        }else if(this.type === 'uint' && event.key == '-') {
          event.preventDefault();
        }else{
          event.stopPropagation();
        }
      }
    }

    if (countMinus > 0) {
      if(event.key !== 'ArrowRight' && event.key !== 'Backspace' && event.key !== 'Delete' && !(event.key.toLowerCase() == 'c' && event.ctrlKey) && !(event.key.toLowerCase() == 'a' && event.ctrlKey) && !(event.key.toLowerCase() == 'v' && event.ctrlKey) && !(event.key.toLowerCase() == 'x' && event.ctrlKey)  && !(event.key.toLowerCase() == 'z' && event.ctrlKey)){
          event.preventDefault();
      }
    }

//#### เอาไว้ใช้กรณีที่จะไม่ให้กด Function in keyboard
    // if(this.type == 'int'){
    //   if(countDot > 0){
    //     event.preventDefault();
    //   }
    // }else if(this.type == 'float'){
    //   if(countDot > 1){
    //     event.preventDefault();
    //   }
    //   if(checkDot == true){
    //     event.preventDefault();
    //   }
    // }

  }

  countMinusFromStart(inputString: string): number {
    let count = 0;
    for (let i = 1; i < inputString.length; i++) {
      if (inputString[i] === '-') {
        count++;
      }
    }
    return count;
  }

  countDot(inputString: string): number {
    let count = 0;
    for (let i = 0; i < inputString.length; i++) {
      if (inputString[i] === '.') {
        count++;
      }
    }
    return count;
  }

  checkDotFromStart(inputString: string , key : string, index): boolean {
    if(key === '.'){
      if(inputString[0] == '.'){
        return true;
      }else if(isNaN(Number(inputString[index-1]))){
        return true;
      }
    }
    return false;
  }
  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    const pastedInput: string = event.clipboardData?.getData('text/plain') || '';
    if(this.type == 'int'){
      if(pastedInput.includes('.')){
        event.preventDefault();
      }
    }else if(this.type == 'uint'){
      if(pastedInput.includes('.') || pastedInput.includes('-')){
        event.preventDefault();
      }
    }

    if (!this.regex.test(pastedInput)) {
      event.preventDefault();
    }
  }

  @HostListener('copy', ['$event'])
  onCopy(event: ClipboardEvent) {
    // Allow copy
  }

  @HostListener('cut', ['$event'])
  onCut(event: ClipboardEvent) {
    // Allow cut
  }
}
