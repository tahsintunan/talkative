import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { EMPTY } from 'rxjs';
import { AuthServiceMock } from 'src/app/core/mock-services/auth.service.mock';
import { FollowServiceMock } from 'src/app/core/mock-services/follow.service.mock';
import { NotificationServiceMock } from 'src/app/core/mock-services/notification.service.mock';
import { UserServiceMock } from 'src/app/core/mock-services/user.service.mock';
import { AuthService } from 'src/app/core/services/auth.service';
import { FollowService } from 'src/app/core/services/follow.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { UserService } from 'src/app/core/services/user.service';
import { PasswordUpdateDialogComponent } from '../../ui/password-update-dialog/password-update-dialog.component';
import { ProfileImageUploaderDialogComponent } from '../../ui/profile-image-uploader-dialog/profile-image-uploader-dialog.component';
import { ProfileUpdateDialogComponent } from '../../ui/profile-update-dialog/profile-update-dialog.component';
import { ProfileComponent } from './profile.component';

describe('Profile component', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let dialogSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientModule,
        MatDialogModule,
        MatSnackBarModule,
      ],
      declarations: [ProfileComponent],
      providers: [
        { provide: FollowService, useClass: FollowServiceMock },
        { provide: AuthService, useClass: AuthServiceMock },
        { provide: UserService, useClass: UserServiceMock },
        { provide: NotificationService, useClass: NotificationServiceMock },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    dialogSpy = spyOn(component.dialog, 'open').and.returnValue({
      afterClosed: () => EMPTY,
    } as any);
    fixture.detectChanges();
  });

  it('should create the profile item', () => {
    expect(ProfileComponent).toBeTruthy();
  });

  it('should open profile edit dialog', () => {
    component.onProfileEdit();
    expect(dialogSpy).toHaveBeenCalledWith(ProfileUpdateDialogComponent, {
      width: '500px',
      data: component.profile,
    });
  });

  it('should open password edit dialog', () => {
    component.onPasswordEdit();
    expect(dialogSpy).toHaveBeenCalledWith(PasswordUpdateDialogComponent, {
      width: '500px',
      data: component.profile,
    });
  });

  it('should open profile image dialog', () => {
    component.onProfileImageEdit();
    expect(dialogSpy).toHaveBeenCalledWith(
      ProfileImageUploaderDialogComponent,
      {
        width: '500px',
      }
    );
  });

  it('should open cover image dialog', () => {
    component.onCoverImageEdit();
    expect(dialogSpy).toHaveBeenCalledWith(
      ProfileImageUploaderDialogComponent,
      {
        width: '500px',
        data: { isCover: true },
      }
    );
  });
});
