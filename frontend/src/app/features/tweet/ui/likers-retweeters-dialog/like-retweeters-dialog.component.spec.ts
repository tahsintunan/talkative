import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatDialogModule,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { TweetServiceMock } from 'src/app/core/mock-services/tweet.service.mock';
import { TweetService } from 'src/app/core/services/tweet.service';
import { LikersRetweetersDialogComponent } from './likers-retweeters-dialog.component';

describe('Like retweeters dialog component', () => {
  let component: LikersRetweetersDialogComponent;
  let fixture: ComponentFixture<LikersRetweetersDialogComponent>;
  let dialogCloseSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [LikersRetweetersDialogComponent],
      providers: [
        { provide: TweetService, useClass: TweetServiceMock },
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: { close: () => {} } },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LikersRetweetersDialogComponent);
    component = fixture.componentInstance;
    dialogCloseSpy = spyOn(component.dialogRef, 'close');
    fixture.detectChanges();
  });

  it('should create the Like retweeters dialog dialog', () => {
    expect(LikersRetweetersDialogComponent).toBeTruthy();
  });

  it('should get liked users', fakeAsync(() => {
    component.getLikers();
    tick();
    expect(component.users).toHaveSize(3);
  }));

  it('should get retweeters', fakeAsync(() => {
    component.getRetweeters();
    tick();
    expect(component.users).toHaveSize(3);
  }));

  it('should increase page number on scroll', () => {
    component.pagination.pageNumber = 1;
    const container = fixture.debugElement.query(By.css('section'));
    container.triggerEventHandler('scrolled', null);
    expect(component.pagination.pageNumber).toEqual(2);
  });
});
