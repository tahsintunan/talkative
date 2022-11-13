import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserManagementComponent } from './user-management.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EMPTY, of } from 'rxjs';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/core/services/admin.service';
import { By } from '@angular/platform-browser';
describe('User Management', () => {
  let component: UserManagementComponent;
  let fixture: ComponentFixture<UserManagementComponent>;
  let serviceStub: any;
  beforeEach(async () => {

    serviceStub = {
      searchUser: (value: string, page: number) => of([{
        userId: 1,
        username: "siam",
        email:"random@gmail.com"
      },{
        userId: 2,
        username: "siam1",
        email:"random@gmail.com"
        }]),
      getUserList: () => of([{
        userId: 1,
        username: "siam",
        email:"random@gmail.com"
      },{
        userId: 2,
        username: "siam1",
        email:"random@gmail.com"
      }])
    }

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule
      ],
      declarations: [
        UserManagementComponent
      ],
      providers: [
        AuthService,
        MatDialog,
        {
          provide: Router, useClass: class { navigate = jasmine.createSpy("navigate"); }
        },
        {
          provide:AdminService, useValue:serviceStub
        }
      ],
      schemas:[NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges()
  })
  it('should create the user management page', () => {
    expect(UserManagementComponent).toBeTruthy()
  });


  it('should open mat dialog on editUser()', fakeAsync(() => {
   const dialogSpy = spyOn(component.dialog, 'open').and.returnValue({afterClosed: () => EMPTY} as any);
    component.editUser({
      username: "siam",
      email: "random@gmail.com",
    });

   expect(dialogSpy).toHaveBeenCalled();
  }))

  it(`should navigate to user profile`, () => {
    let router = fixture.debugElement.injector.get(Router);
    component.onRowClick({
      userId: "1",
      username: "siam",
      email: "random@gmail.com",
      dateOfBirth:new Date(1998,11,12)
    })
   expect(router.navigate).toHaveBeenCalledWith(['/profile','1']);
  });

  it('should increase page number by onScroll()', () => {
    component.pagination.pageNumber = 1;
    const container = fixture.debugElement.query(By.css('section'));
    container.triggerEventHandler('scrolled', null)
    expect(component.pagination.pageNumber).toEqual(2)
  })
});
