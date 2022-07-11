import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup
  displayError: boolean;

  constructor(
    private _formBuilder: FormBuilder,
    private _authService: AuthService,
    private _cookieService: CookieService,
    private _router: Router
  ) { }

  ngOnInit(): void {
    this._cookieService.delete('accessToken')
    this.initForm()

  }

  initForm() {
    this.loginForm = this._formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', Validators.required]
    })
  }

  login() {
    let loginBody = {
      ...this.loginForm.getRawValue()
    }

    this._authService.login(loginBody).subscribe({
      next: res => {
        let accessToken = res['accessToken']
        this._cookieService.set('accessToken', accessToken)
        this._router.navigate(['user'])
      },
      error: err => {
        console.log(err);

      }
    })
  }

}
