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
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { EMPTY } from 'rxjs';
import { AuthServiceMock } from 'src/app/core/mock-services/auth.service.mock';
import { NotificationServiceMock } from 'src/app/core/mock-services/notification.service.mock';
import { SearchServiceMock } from 'src/app/core/mock-services/search.service.mock';
import { UserServiceMock } from 'src/app/core/mock-services/user.service.mock';
import { AuthService } from 'src/app/core/services/auth.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { SearchService } from 'src/app/core/services/search.service';
import { UserService } from 'src/app/core/services/user.service';
import { NavbarComponent } from './navbar.component';

describe('Navbar', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [NavbarComponent],
      providers: [
        { provide: UserService, useClass: UserServiceMock },
        { provide: SearchService, useClass: SearchServiceMock },
        { provide: AuthService, useClass: AuthServiceMock },
        { provide: NotificationService, useClass: NotificationServiceMock },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the navbar', () => {
    expect(NavbarComponent).toBeTruthy();
  });

  it('should open create tweet dialog', () => {
    const dialogSpy = spyOn(component.dialog, 'open').and.returnValue({
      afterClosed: () => EMPTY,
    } as any);

    component.openCreatePostDialog();

    expect(dialogSpy).toHaveBeenCalled();
  });

  it('should redirect to login page on logout', fakeAsync(() => {
    let router = TestBed.inject(Router);
    const navigateSpy = spyOn(router, 'navigate');
    component.onLogout();
    tick();
    expect(navigateSpy).toHaveBeenCalledWith(['/auth/signin']);
  }));
});
