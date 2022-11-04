import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UtilityService } from 'src/app/shared/services/utility.service';

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
    this.authService
      .signup(this.formData.getRawValue())
      .subscribe(() =>
        this.signinUserAfterSignUp(
          this.formData.get('username')?.value,
          this.formData.get('password')?.value
        )
      );
  }

  signinUserAfterSignUp(username: string, password: string) {
    this.authService
      .signin({ username, password })
      .subscribe(() => this.router.navigate(['/'], { replaceUrl: true }));
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
