import { HttpClientModule } from '@angular/common/http';
import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthServiceMock } from 'src/app/core/mock-services/auth.service.mock';
import { AuthService } from 'src/app/core/services/auth.service';
import { ForgotPasswordDialogComponent } from './forgot-password-dialog.component';

describe('Forgot password dialog Component', () => {
  let component: ForgotPasswordDialogComponent;
  let fixture: ComponentFixture<ForgotPasswordDialogComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
      ],
      declarations: [ForgotPasswordDialogComponent],
      providers: [
        { provide: AuthService, useClass: AuthServiceMock },
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: { close: () => {} } },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgotPasswordDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the forgot password dialog component', () => {
    expect(component).toBeTruthy();
  });

  it('should submit form', fakeAsync(() => {
    component.formData.reset();
    component.formData.setValue({
      email: 'random@gmail.com',
    });

    spyOn(component, 'onSubmit');
    fixture.debugElement
      .query(By.css('form'))
      .triggerEventHandler('ngSubmit', null);
    fixture.detectChanges();
    tick();
    expect(component.onSubmit).toHaveBeenCalled();
  }));

  it('should have an invalid form', () => {
    component.formData.reset();
    component.formData.setValue({
      email: 'random',
    });
    expect(component.formData.get('email')?.errors != null).toBe(true);
  });
});
