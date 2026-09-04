import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable, isObservable, from, of } from 'rxjs';
import * as _ from "lodash";

export interface CanComponentDeactivate {
  canDeactivate: () => boolean | Promise<boolean> | Observable<boolean>;
}

@Injectable({
  providedIn: 'root'
})
export class ConfirmExitGuard implements CanDeactivate<CanComponentDeactivate> {
  canDeactivate(component: CanComponentDeactivate): Observable<boolean> {
    const result = component.canDeactivate();

    if (isObservable(result)) {
      return result;
    } else if (result instanceof Promise) {
      return from(result);
    } else {
      return of(result);
    }
  }
}
