import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import {
  MatSnackBarModule,
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { NotificationServiceMock } from 'src/app/core/mock-services/notification.service.mock';
import { NotificationService } from 'src/app/core/services/notification.service';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
import { NotificationSnackbarComponent } from './notification-snackbar.component';

describe('Notification snackbar component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule, MatSnackBarModule],
      providers: [
        {
          provide: NotificationService,
          useClass: NotificationServiceMock,
        },
        { provide: MatSnackBarRef, useValue: { close: () => {} } },
        { provide: MAT_SNACK_BAR_DATA, useValue: '' },
      ],
      declarations: [NotificationSnackbarComponent, TimeAgoPipe],
    }).compileComponents();
  });

  it('should create the notification snackbar component', () => {
    const fixture = TestBed.createComponent(NotificationSnackbarComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
