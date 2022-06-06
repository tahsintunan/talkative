import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { catchError } from 'rxjs';
import { AuthService } from '../auth.service';

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
    private _authService: AuthService
  ) {}

  ngOnInit(): void {
    this.signupForm = this._formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      dateOfBirth: ['', [Validators.required, this.ageValidator]],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#.\$%\^&\*])(?=.{8,})/
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

  findStrengthOfPassword() {
    if (this.signupForm.controls['password']) {
      let password: string = this.signupForm.controls['password'].value;
    }
  }

  signup() {
    const formValues = this.signupForm.getRawValue();

    this._authService.signup(formValues).subscribe({
      next: (res) => this.showAlert('Signup Sucessful!', '#0de057'),
      error: (err) => {
        let error = err.error;
        console.log(error);
        if (error.statusCode === 400) {
          this.showAlert(error.message, '#e60d00');
          return;
        }
        this.showAlert('Signup unsuccessful!', '#e60d00');
      },
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
