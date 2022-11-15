import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from 'src/app/core/services/auth.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { SearchService } from 'src/app/core/services/search.service';
import { UserService } from 'src/app/core/services/user.service';
import { HomeComponent } from './home.component';
import { SearchChangeModel } from '../../../../core/models/search.model';
import { SearchServiceMock } from 'src/app/core/mock-services/search.service.mock';
import { NotificationServiceMock } from 'src/app/core/mock-services/notification.service.mock';
import { UserServiceMock } from 'src/app/core/mock-services/user.service.mock';
import { AuthServiceMock } from 'src/app/core/mock-services/auth.service.mock';
describe('Home', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule, MatSnackBarModule],
      declarations: [HomeComponent],
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
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the home page', () => {
    expect(HomeComponent).toBeTruthy();
  });

  it('should return empty array when search box is empty', fakeAsync(() => {
    const searchSettingsModel: SearchChangeModel = {
      value: '',
      pagination: { pageNumber: 1, itemCount: 20 },
    };
    component.onSearchChange(searchSettingsModel);
    tick();
    expect(component.searchResults.length).toBe(0);
  }));

  it('should return users when no # is in value', fakeAsync(() => {
    const searchSettingsModel: SearchChangeModel = {
      value: 'siam',
      pagination: { pageNumber: 1, itemCount: 20 },
    };
    component.onSearchChange(searchSettingsModel);
    tick();
    expect(component.searchResults.length).toBe(2);
  }));

  it('should return tweets when # is in value', fakeAsync(() => {
    const searchSettingsModel: SearchChangeModel = {
      value: '#siam',
      pagination: { pageNumber: 1, itemCount: 20 },
    };
    component.onSearchChange(searchSettingsModel);
    tick();
    expect(component.searchResults.length).toEqual(3);
  }));
});
