import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import * as _  from 'lodash';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router)
  const token = sessionStorage.getItem('token');
  if(!_.isNil(token)){
    return true;
  }else{
    router.navigate(['login'])
  }
};
