import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  MatSnackBarModule,
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';

import { AlertSnackbarComponent } from './alert-snackbar.component';

describe('AlertSnackbarComponent', () => {
  let component: AlertSnackbarComponent;
  let fixture: ComponentFixture<AlertSnackbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatSnackBarModule],
      declarations: [AlertSnackbarComponent],
      providers: [
        { provide: MatSnackBarRef, useValue: { close: () => {} } },
        { provide: MAT_SNACK_BAR_DATA, useValue: '' },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AlertSnackbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
