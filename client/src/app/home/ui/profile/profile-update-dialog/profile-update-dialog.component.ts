import { Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UtilityService } from 'src/app/shared/services/utility.service';

interface ProfileUpdateDialogData {
  username: string;
  email: string;
  dateOfBirth: string;
}

@Component({
  selector: 'app-profile-update-dialog',
  templateUrl: './profile-update-dialog.component.html',
  styleUrls: ['./profile-update-dialog.component.css'],
})
export class ProfileUpdateDialogComponent implements OnInit {
  formData: FormGroup = this.formBuilder.group({
    username: [this.data?.username, [Validators.required]],
    email: [this.data?.email, [Validators.required, Validators.email]],
    dateOfBirth: [
      this.data?.dateOfBirth,
      [Validators.required, this.ageValidator.bind(this)],
    ],
  });

  constructor(
    private utilityService: UtilityService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ProfileUpdateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: ProfileUpdateDialogData
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.dialogRef.close(this.formData.getRawValue());
  }

  private ageValidator(control: AbstractControl): ValidationErrors | null {
    return this.utilityService.getAge(control.value) < 18
      ? { underage: true }
      : null;
  }
}
