import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertSnackbarComponent } from './alert-snackbar.component';

describe('AlertSnackbarComponent', () => {
  let component: AlertSnackbarComponent;
  let fixture: ComponentFixture<AlertSnackbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AlertSnackbarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlertSnackbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
