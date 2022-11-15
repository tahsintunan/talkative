import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatDialogModule,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { EMPTY } from 'rxjs';
import { ForgotPasswordDialogComponent } from 'src/app/shared/ui/forgot-password-dialog/forgot-password-dialog.component';
import { PasswordUpdateDialogComponent } from './password-update-dialog.component';

describe('Password update dialog component', () => {
  let component: PasswordUpdateDialogComponent;
  let fixture: ComponentFixture<PasswordUpdateDialogComponent>;
  let dialogSpy: any;
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
      declarations: [PasswordUpdateDialogComponent],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: { close: () => {} } },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PasswordUpdateDialogComponent);
    component = fixture.componentInstance;
    dialogSpy = spyOn(component.dialog, 'open').and.returnValue({
      afterClosed: () => EMPTY,
    } as any);
    dialogCloseSpy = spyOn(component.dialogRef, 'close');
    fixture.detectChanges();
  });

  it('should create the update password dialog', () => {
    expect(PasswordUpdateDialogComponent).toBeTruthy();
  });

  it('should have an invalid form', () => {
    component.formData.setValue({
      userId: '1',
      oldPassword: '2',
      newPassword: '1234',
      confirmPassword: '123456',
    });
    expect(
      component.formData.get('confirmPassword')?.errors!['mismatch'] &&
        component.formData.get('newPassword')?.errors!['pattern'] != null &&
        component.formData.get('newPassword')?.errors!['minlength'] != null
    ).toBe(true);
  });

  it('should have a valid form', () => {
    component.formData.setValue({
      userId: '1',
      oldPassword: '2',
      newPassword: '123456aA!',
      confirmPassword: '123456aA!',
    });
    expect(
      !component.formData.get('confirmPassword')?.errors &&
        !component.formData.get('newPassword')?.errors &&
        !component.formData.get('newPassword')?.errors
    ).toBe(true);
  });

  it('should open forget password dialog', () => {
    component.onForgotPassword();
    expect(dialogSpy).toHaveBeenCalledWith(ForgotPasswordDialogComponent, {
      width: '500px',
    });
  });

  it('should close password edit dialog on form submit', () => {
    component.onSubmit();
    expect(dialogCloseSpy).toHaveBeenCalledWith(
      component.formData.getRawValue()
    );
  });

  it('should close password edit dialog on close', () => {
    component.onClose();
    expect(dialogCloseSpy).toHaveBeenCalled();
  });
});
