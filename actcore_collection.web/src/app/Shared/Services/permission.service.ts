import { Injectable } from '@angular/core';
import { BehaviorSubject, filter, lastValueFrom, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  permission$ : BehaviorSubject<any[]> = new BehaviorSubject<any[]>(null);
  menu$ : BehaviorSubject<any[]> = new BehaviorSubject<any[]>(null);
  constructor() { }


  setPermission(data){
    this.permission$.next(data);
  }

  getPermission(){
    return this.permission$.asObservable();
  }


  setMenu(data){
    this.menu$.next(data);
  }

  getMenu(){
    return this.menu$.asObservable();
  }
}
