import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  hidePassword: boolean = true;
  hideConfirmPassword: boolean = true;

  signupForm: FormGroup = this.formBuilder.group({});

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

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
      { validator: this.confirmPasswordValidator }
    );
  }

  submitForm() {
    const formValues = this.signupForm.getRawValue();

    console.log(formValues);

    this.authService.signup(formValues).subscribe({
      next: (res) => {
        this.router.navigate(['home']);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  private ageValidator(control: AbstractControl): ValidationErrors | null {
    if (
      control.value &&
      new Date().getFullYear() - new Date(control.value).getFullYear() < 18
    ) {
      return { underage: true };
    }
    return null;
  }

  private confirmPasswordValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    return password !== confirmPassword ? { passwordMismatch: true } : null;
  }
}
