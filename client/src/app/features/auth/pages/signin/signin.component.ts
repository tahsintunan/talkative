import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { ForgotPasswordDialogComponent } from 'src/app/shared/ui/forgot-password-dialog/forgot-password-dialog.component';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
})
export class SigninComponent implements OnInit {
  hidePassword: boolean = true;

  formData: FormGroup = this.formBuilder.group({
    username: [null, [Validators.required]],
    password: [null, [Validators.required]],
  });

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.authService
      .signin(this.formData.getRawValue())
      .subscribe(() => this.router.navigate(['/'], { replaceUrl: true }));
  }

  onForgotPasswordClick() {
    this.dialog.open(ForgotPasswordDialogComponent, {
      width: '500px',
    });
  }
}
