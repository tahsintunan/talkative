import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  MatDialogModule,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { EMPTY } from 'rxjs';
import { TweetServiceMock } from 'src/app/core/mock-services/tweet.service.mock';
import { TweetService } from 'src/app/core/services/tweet.service';
import { PostMakerDialogComponent } from '../../ui/post-maker-dialog/post-maker-dialog.component';
import { FeedComponent } from './feed.component';

describe('Feed component', () => {
  let component: FeedComponent;
  let fixture: ComponentFixture<FeedComponent>;
  let dialogSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [FeedComponent],
      providers: [
        { provide: TweetService, useClass: TweetServiceMock },
        {
          provide: Router,
          useClass: class {
            navigate = jasmine.createSpy('navigate');
          },
        },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedComponent);
    component = fixture.componentInstance;
    dialogSpy = spyOn(component.dialog, 'open').and.returnValue({
      afterClosed: () => EMPTY,
    } as any);
    fixture.detectChanges();
  });

  it('should create the feed component', () => {
    expect(FeedComponent).toBeTruthy();
  });

  it('should open dialog on create post', () => {
    component.onCreatePost();
    expect(dialogSpy).toHaveBeenCalledWith(PostMakerDialogComponent, {
      width: '500px',
    });
  });

  it('should increase page number on scroll', () => {
    component.pagination.pageNumber = 1;
    const container = fixture.debugElement.query(By.css('section'));
    container.triggerEventHandler('scrolled', null);
    expect(component.pagination.pageNumber).toEqual(2);
  });

  it('should go to top of the page on onScrollToTop()', () => {
    component.pagination.pageNumber = 10;
    component.onScrollToTop();
    expect(component.pagination.pageNumber).toEqual(1);
  });

  it('should navigate to search with hashtags', () => {
    let router = fixture.debugElement.injector.get(Router);
    component.onTrendingHashtagClick('hashtagValue');
    expect(router.navigate).toHaveBeenCalledWith(['/search'], {
      queryParams: { type: 'hashtag', value: 'hashtagValue' },
    });
  });
});
