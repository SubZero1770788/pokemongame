import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(public _accS: AccountService) 
  {
   
  }

  canActivate(): Observable<boolean> {
    return  this._accS.currentUsersrc$.pipe(
      map(user => {
        if (user) return true;
        else {
          return false;
        }
      } 
    ))
  }
}
