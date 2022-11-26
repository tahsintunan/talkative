import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatDialogModule,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { ProfileUpdateDialogComponent } from './profile-update-dialog.component';

describe('Profile update dialog component', () => {
  let component: ProfileUpdateDialogComponent;
  let fixture: ComponentFixture<ProfileUpdateDialogComponent>;
  let dialogCloseSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [ProfileUpdateDialogComponent],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: { close: () => {} } },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileUpdateDialogComponent);
    component = fixture.componentInstance;
    dialogCloseSpy = spyOn(component.dialogRef, 'close');
    fixture.detectChanges();
  });

  it('should create the update profile dialog', () => {
    expect(ProfileUpdateDialogComponent).toBeTruthy();
  });

  it('should have an invalid form', () => {
    component.formData.setValue({
      userId: '1',
      username: 'p',
      email: 'random',
      dateOfBirth: new Date().toISOString().substring(0, 10),
    });

    expect(
      component.formData.get('email')?.errors!['email'] &&
        component.formData.get('username')?.errors!['minlength'] != null &&
        component.formData.get('dateOfBirth')?.errors!['underage']
    ).toBe(true);
  });

  it('should not take more than 20 characters for username', () => {
    component.formData.setValue({
      userId: '1',
      username: 'atwentylengthcharacter',
      email: 'random@gmail.com',
      dateOfBirth: new Date(1998, 10, 10).toISOString().substring(0, 10),
    });

    expect(
      !component.formData.get('email')?.errors &&
        component.formData.get('username')?.errors!['maxlength'] != null &&
        !component.formData.get('dateOfBirth')?.errors
    ).toBe(true);
  });

  it('should have a valid form', () => {
    component.formData.setValue({
      userId: '1',
      username: 'siam',
      email: 'random@gmail.com',
      dateOfBirth: new Date(1998, 11, 12).toISOString().substring(0, 10),
    });

    expect(
      !component.formData.get('email')?.errors &&
        !component.formData.get('username')?.errors &&
        !component.formData.get('dateOfBirth')?.errors
    ).toBe(true);
  });

  it('should close profile edit dialog on form submit', () => {
    component.onSubmit();
    expect(dialogCloseSpy).toHaveBeenCalledWith(
      component.formData.getRawValue()
    );
  });

  it('should close profile edit dialog on close', () => {
    component.onClose();
    expect(dialogCloseSpy).toHaveBeenCalled();
  });
});
