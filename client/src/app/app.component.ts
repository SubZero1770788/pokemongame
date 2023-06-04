import { Component, OnInit } from '@angular/core';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(public _accServ: AccountService){}
  
  ngOnInit(): void {
    this.setUser();
  }

  setUser()
  {
    const u = localStorage.getItem('userData');
    if (!u) return;
    const user = JSON.parse(u);
    this._accServ.setCurrentUser(user);
  }
}
