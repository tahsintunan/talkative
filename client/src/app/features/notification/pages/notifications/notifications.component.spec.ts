import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { RouterTestingModule } from '@angular/router/testing';
import { NotificationServiceMock } from 'src/app/core/mock-services/notification.service.mock';
import { NotificationService } from 'src/app/core/services/notification.service';
import { NotificationsComponent } from './notifications.component';

describe('Notification Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule, MatDialogModule],
      declarations: [NotificationsComponent],
      providers: [
        { provide: NotificationService, useClass: NotificationServiceMock },
      ],
    }).compileComponents();
  });

  it('should create the notification component', () => {
    const fixture = TestBed.createComponent(NotificationsComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
