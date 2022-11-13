import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { SignupComponent } from './signup.component';
import { DebugElement, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/core/services/auth.service';
import { By } from '@angular/platform-browser';
describe('Signup', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatSnackBarModule
      ],
      declarations: [
        SignupComponent
      ],
      providers:[AuthService],
      schemas:[NO_ERRORS_SCHEMA]
    }).compileComponents();
  });
  beforeEach(() => {
    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges()
  })
  it('should create the signup page', () => {
    expect(SignupComponent).toBeTruthy()
  });

  it('form should be invalid', () => {
    component.formData.reset();
    component.formData.setValue({
      "username": "siam",
      "email": "random",
      "password": "123456A",
      "confirmPassword": "12345!",
      "dateOfBirth":new Date().toISOString().substring(0,10)
    })

    expect(component.formData.get('email')?.errors!['email']
      && component.formData.get('confirmPassword')?.errors!['mismatch']
      && component.formData.get('dateOfBirth')?.errors!['underage']
      && component.formData.get('password')?.errors!['pattern']!=null
      && component.formData.get('password')?.errors!['minlength']!=null)
      .toBe(true);
  })

  it('form should be valid', () => {
    component.formData.reset();
    component.formData.setValue({
      "username": "siam",
      "email": "random@gmail.com",
      "password": "123456Aa!",
      "confirmPassword": "123456Aa!",
      "dateOfBirth":new Date(1998,11,12).toISOString().substring(0,10)
    })

    expect(component.formData.get('email')?.errors==null
      && component.formData.get('confirmPassword')?.errors==null
      && component.formData.get('dateOfBirth')?.errors==null
      && component.formData.get('password')?.errors==null
      && component.formData.get('password')?.errors==null)
      .toBe(true);
  })

  it('should submit form', fakeAsync(() => {
    component.formData.reset();
    component.formData.setValue({
      "username": "siam398",
      "email": "random@gmail.com",
      "password": "123456Aa!",
      "confirmPassword": "123456Aa!",
      "dateOfBirth":new Date(1998,11,11).toISOString().substring(0,10)
    })

    spyOn(component, "onSubmit")
    fixture.debugElement.query(By.css('form')).triggerEventHandler('ngSubmit', null);
    fixture.detectChanges()
    tick(1000)
    expect(component.onSubmit).toHaveBeenCalled();
  }))

});
