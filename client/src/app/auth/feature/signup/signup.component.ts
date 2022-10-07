import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  hidePassword: boolean = true;
  hideConfirmPassword: boolean = true;

  formData: FormGroup = this.formBuilder.group(
    {
      username: [null, [Validators.required]],
      email: [null, [Validators.email, Validators.required]],
      dateOfBirth: [null, [Validators.required, this.ageValidator.bind(this)]],
      password: [
        null,
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#.,;+_=\/\\\$%\^&\*\-])(?=.{8,})/
          ),
          Validators.minLength(8),
        ],
      ],
      confirmPassword: [null, [Validators.required]],
    },
    {
      validators: this.confirmPasswordValidator('password', 'confirmPassword'),
    }
  );

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private utilityService: UtilityService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    const formValues = this.formData.getRawValue();

    this.authService.signup(formValues).subscribe({
      next: () => {
        let password = formValues.password;
        let username = formValues.username;
        this.signinUserAfterSignUp(username, password);
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
      },
    });
  }

  private ageValidator(control: AbstractControl): ValidationErrors | null {
    return this.utilityService.getAge(control.value) < 18
      ? { underage: true }
      : null;
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
