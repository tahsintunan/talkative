import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-forgot-password-dialog',
  templateUrl: './forgot-password-dialog.component.html',
  styleUrls: ['./forgot-password-dialog.component.css'],
})
export class ForgotPasswordDialogComponent implements OnInit {
  formData: FormGroup = this.formBuilder.group({
    email: [null, [Validators.required, Validators.email]],
  });

  success: boolean = false;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ForgotPasswordDialogComponent>
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    this.success = false;

    this.authService
      .forgotPassword(this.formData.get('email')?.value)
      .subscribe((res) => {
        this.success = true;
      });
  }
}
