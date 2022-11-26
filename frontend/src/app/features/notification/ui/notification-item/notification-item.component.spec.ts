import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
import { NotificationItemComponent } from './notification-item.component';

describe('Notification item component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule],
      declarations: [NotificationItemComponent, TimeAgoPipe],
    }).compileComponents();
  });

  it('should create the notification item component', () => {
    const fixture = TestBed.createComponent(NotificationItemComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
