import {Injectable} from '@angular/core';
import { BehaviorSubject, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShareService {


  availableLang$ : BehaviorSubject<any> = new BehaviorSubject<any>(null);



  constructor() {}

  getToken(): string {
    return sessionStorage.getItem('token')!;
  }


  getUser(): string {
    return localStorage.getItem('User')!;
  }

  setAvailableLang(data){
    this.availableLang$.next(data);
  }

  getAvailableLang(){
    return  this.availableLang$.asObservable();
  }
}
