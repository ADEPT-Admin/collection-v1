import { Injectable } from '@angular/core';
import { Subject, timer } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppLoaderService {

  loader$ : Subject<boolean> = new Subject();
  ids : any[] = [];

  constructor() { }
  show(){

    const tooltip = document.querySelectorAll('.e-tooltip-wrap.e-popup.e-popup-open');
    tooltip.forEach(item => {
      item.setAttribute('style' , 'display:none')
      // item.remove();
    });
    const id = this.uuidv4();
    this.ids.push(id);
    this.loader$.next(this.ids.length > 0);
    return id;


  }

  hide(id){
    const index = this.ids.indexOf(id);
    if(index > -1){
      const x = this.ids.splice(index, 1);
      this.updateLoaderState();
    }


  }

  clear(){
    this.loader$.next(false);
  }

  hideAll() {
    this.ids = [];
    this.updateLoaderState();
  }

  updateLoaderState(): void {
    if(this.ids.length > 0){
      this.loader$.next(true);
    }else {
      timer(200).subscribe(() => {
        this.loader$.next(false);
    })
    }
  }

  getLoader(){
    return this.loader$.asObservable();
  }


  private uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    .replace(/[xy]/g, function (c) {
        const r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
  }
}
