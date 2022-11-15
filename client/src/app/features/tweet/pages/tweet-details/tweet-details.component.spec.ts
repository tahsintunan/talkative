import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { EMPTY, Observable, of } from 'rxjs';
import { ActivatedRouteMock } from 'src/app/core/mock-services/activated-route.mock';
import { TweetServiceMock } from 'src/app/core/mock-services/tweet.service.mock';
import { TweetService } from 'src/app/core/services/tweet.service';
import { TweetDetailsComponent } from './tweet-details.component';

describe('Tweet details component', () => {
  let component: TweetDetailsComponent;
  let fixture: ComponentFixture<TweetDetailsComponent>;
  let dialogSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [TweetDetailsComponent],
      providers: [{ provide: TweetService, useClass: TweetServiceMock }],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TweetDetailsComponent);
    component = fixture.componentInstance;
    dialogSpy = spyOn(component.dialog, 'open').and.returnValue({
      afterClosed: () => EMPTY,
    } as any);
    fixture.detectChanges();
  });

  it('should create the tweet details component', () => {
    expect(TweetDetailsComponent).toBeTruthy();
  });
});
