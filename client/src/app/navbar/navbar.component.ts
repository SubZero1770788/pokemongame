import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { AccountService } from '../services/account.service';
import { PokemonsService } from '../services/pokemons.service';
import { ShopService } from '../services/shop.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  logged: boolean = true;
  points: any;
  loginForm: FormGroup = new FormGroup({});
  constructor(public _accServ: AccountService, public _pokeS: PokemonsService, public _shopS: ShopService, private router: Router) { }

  ngOnInit(): void {
   this._shopS.pointPasser.subscribe(
    n => {
      this.points-=n;
   })
   this.createLoginForm();
   this._accServ.pointEmitter.subscribe(
    r => {
      this.points = r;
    }
   )
  }

  toggleLogin()
  {
    this._accServ.login(this.loginForm.value).subscribe({
      next: r => {
        this.logged = true;
        this.navigate();
      },
      error: e => console.log(e),
    })
    this.points = this._accServ.points;
  }

  createLoginForm()
  {
    this.loginForm = new FormGroup({
      UserName: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]),
      Password: new FormControl('', [Validators.required, Validators.minLength(7), Validators.maxLength(12)])
    });
  }

  logout()
  {
    this._accServ.logout();
    this.logged = false;
  }

  navigate()
  {
    this.router.navigateByUrl("/wild");
  }

  allPokemons()
  {
    this.router.navigateByUrl("/pokemons");
  }

}
