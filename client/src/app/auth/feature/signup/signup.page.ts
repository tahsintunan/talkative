import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.page.html',
  styleUrls: ['./signup.page.css'],
})
export class SignupPage implements OnInit {
  hidePassword: boolean = true;
  hideConfirmPassword: boolean = true;

  signupForm: FormGroup = this.formBuilder.group({});

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.signupForm = this.formBuilder.group(
      {
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
        confirmPassword: ['', [Validators.required]],
      },
      {
        validators: this.confirmPasswordValidator(
          'password',
          'confirmPassword'
        ),
      }
    );
  }

  submitForm() {
    const formValues = this.signupForm.getRawValue();

    this.authService.signup(formValues).subscribe({
      next: () => {
        let password = formValues.password;
        let username = formValues.username;
        this.signinUserAfterSignUp(username, password)
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  signinUserAfterSignUp(username: string, password: string) {
    this.authService.signin({ username, password }).subscribe({
      next: () => {
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.log(err);

      }
    })
  }

  private ageValidator(control: AbstractControl): ValidationErrors | null {
    const today = new Date();
    const birthDate = new Date(control.value);
    const month = today.getMonth() - birthDate.getMonth();

    let age = today.getFullYear() - birthDate.getFullYear();
    if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    if (age < 18) {
      return { underage: true };
    }
    return null;
  }

  private confirmPasswordValidator(
    passwordControlName: string,
    confirmPasswordControlName: string
  ) {
    return (formGroup: FormGroup) => {
      const password = formGroup.controls[passwordControlName];
      const confirmPassword = formGroup.controls[confirmPasswordControlName];

      if (confirmPassword.errors && !confirmPassword.errors['mismatch']) {
        return;
      }

      if (password.value !== confirmPassword.value) {
        confirmPassword.setErrors({ mismatch: true });
      } else {
        confirmPassword.setErrors(null);
      }
    };
  }
}
