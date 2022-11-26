import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { FollowServiceMock } from 'src/app/core/mock-services/follow.service.mock';
import { FollowService } from 'src/app/core/services/follow.service';
import { UserListItemComponent } from './user-list-item.component';

describe('User list item component', () => {
  let component: UserListItemComponent;
  let fixture: ComponentFixture<UserListItemComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [UserListItemComponent],
      providers: [{ provide: FollowService, useClass: FollowServiceMock }],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the user list item', () => {
    expect(UserListItemComponent).toBeTruthy();
  });

  it('should follow user', fakeAsync(() => {
    component.user = {
      userId: '1',
      username: 'siam',
    };
    spyOn(component, 'followUser');

    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Follow')
      )
      .triggerEventHandler('click', null);

    expect(component.followUser).toHaveBeenCalled();
  }));

  it('should unfollow user', fakeAsync(() => {
    component.isFollowing = true;
    component.user = {
      userId: '1',
      username: 'siam',
    };
    fixture.detectChanges();
    spyOn(component, 'unfollowUser');

    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Unfollow')
      )
      .triggerEventHandler('click', null);

    expect(component.unfollowUser).toHaveBeenCalled();
  }));
});
