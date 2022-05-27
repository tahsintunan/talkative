import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  signupForm: FormGroup;
  passwordMismatch: boolean;
  constructor(
    private _formBuilder: FormBuilder,
    private _authService: AuthService
  ) { }

  ngOnInit(): void {
    this.signupForm = this._formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      dateOfBirth: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    })
  }

  signup() {
    const formValues = this.signupForm.getRawValue()
    if (formValues.confirmPassword === formValues.password) {
      this.passwordMismatch = false;
      this._authService.signup(formValues).subscribe(res => {
        console.log(res);

      })
    } else {
      this.passwordMismatch = true;
      console.log(this.passwordMismatch);
    }
  }

}
