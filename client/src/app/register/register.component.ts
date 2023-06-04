import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../services/account.service';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponentComponent implements OnInit {
  registerData: any = {};
  registerForm: FormGroup = new FormGroup({});
  constructor(public _accServ: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = new FormGroup({
      UserName: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]),
      Password: new FormControl('', [Validators.required, Validators.minLength(7), Validators.maxLength(12)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchData('Password')]),
      Pokemon: new FormControl('charmander')
    });
    this.registerForm.controls['Password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchData(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
  }

  register()
  {
    this._accServ.registerUser(this.registerForm.value).subscribe({
      next: r => {
        console.log(r);
        this.navigate();
      }
    })
  }

  navigate()
  {
    this.router.navigateByUrl('/wild');
  }
}
