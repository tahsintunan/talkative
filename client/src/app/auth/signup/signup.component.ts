import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { catchError } from 'rxjs';
import { AuthService } from '../auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  signupForm: FormGroup;
  displayError: boolean = false;
  color: any;
  alert: { message: any; color: any; };
  constructor(
    private _formBuilder: FormBuilder,
    private _snackBar: MatSnackBar,
    private _authService: AuthService,
    private _cookieService: CookieService,
    private _router: Router
  ) { }

  ngOnInit(): void {
    this._cookieService.delete('accessToken')
    this.initForm()

  }

  initForm() {
    this.signupForm = this._formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      dateOfBirth: ['', [Validators.required, this.ageValidator]],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#.,;+_=\/\\\$%\^&\*\-])(?=.{8,})/
          ),
          Validators.minLength(8),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.confirmPasswordValidator],
      ],
    });
  }


  signup() {
    const formValues = this.signupForm.getRawValue();

    this._authService.signup(formValues).subscribe({
      next: (res) => {
        let accessToken = res['accessToken']
        this._cookieService.set('accessToken', accessToken)
        this._router.navigate(['user'])
      },
      error: (err) => {
        let error = err.error;
        this.handleError(error)
      },
    });
  }

  private handleError(error) {
    if (error.statusCode === 400) {
      this._snackBar.open(error.message, 'Undo', {
        duration: 2000,
      });
      return;
    }

    this._snackBar.open('Signup unsuccessful!', 'Undo', {
      duration: 2000,
    });
  }

  private ageValidator(control) {
    if (
      control.value &&
      new Date().getFullYear() - new Date(control.value).getFullYear() < 18
    ) {
      return { underage: true };
    }
    return null;
  }

  private confirmPasswordValidator(control: FormGroup) {
    if (control.value) {
      let password = control.parent.controls['password'].value;
      let confirmPassword = control.value;
      if (password !== confirmPassword) {
        return { passwordMismatch: true };
      }
    }
    return null;
  }

  showAlert(message, color) {
    this.alert = {
      message,
      color,
    };
    setTimeout(() => {
      this.alert = null;
    }, 3000);
  }
}
