import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserUpdateReqModel } from 'src/app/home/models/user.model';

@Component({
  selector: 'app-password-update-dialog',
  templateUrl: './password-update-dialog.component.html',
  styleUrls: ['./password-update-dialog.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class PasswordUpdateDialogComponent implements OnInit {
  hideOldPassword: boolean = true;
  hideNewPassword: boolean = true;
  hideConfirmPassword: boolean = true;

  formData: FormGroup = this.formBuilder.group(
    {
      userId: [this.data.userId, Validators.required],
      oldPassword: ['', [Validators.required]],
      newPassword: [
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
        'newPassword',
        'confirmPassword'
      ),
    }
  );

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<PasswordUpdateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserUpdateReqModel
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.dialogRef.close(this.formData.getRawValue());
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
