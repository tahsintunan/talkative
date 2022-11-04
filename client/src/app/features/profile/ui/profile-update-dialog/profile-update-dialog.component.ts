import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserUpdateReqModel } from 'src/app/core/models/user.model';
import { UtilityService } from 'src/app/shared/services/utility.service';

@Component({
  selector: 'app-profile-update-dialog',
  templateUrl: './profile-update-dialog.component.html',
  styleUrls: ['./profile-update-dialog.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ProfileUpdateDialogComponent implements OnInit {
  formData: FormGroup = this.formBuilder.group({
    userId: [this.data.userId, Validators.required],
    username: [
      this.data.username,
      [Validators.required, Validators.minLength(3), Validators.maxLength(20)],
    ],
    email: [this.data.email, [Validators.required, Validators.email]],
    dateOfBirth: [
      this.data.dateOfBirth,
      [Validators.required, this.ageValidator.bind(this)],
    ],
  });

  constructor(
    private utilityService: UtilityService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ProfileUpdateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserUpdateReqModel
  ) {}

  ngOnInit(): void {}

  onClose() {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.dialogRef.close(this.formData.getRawValue());
  }

  private ageValidator(control: AbstractControl): ValidationErrors | null {
    return this.utilityService.getAge(control.value) < 18
      ? { underage: true }
      : null;
  }
}
