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
import { FollowServiceMock } from 'src/app/core/mock-services/follow.service.mock';
import { TweetServiceMock } from 'src/app/core/mock-services/tweet.service.mock';
import { TweetModel } from 'src/app/core/models/tweet.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { FollowService } from 'src/app/core/services/follow.service';
import { TweetService } from 'src/app/core/services/tweet.service';
import { LikersRetweetersDialogComponent } from 'src/app/features/tweet/ui/likers-retweeters-dialog/likers-retweeters-dialog.component';
import { PostMakerDialogComponent } from 'src/app/features/tweet/ui/post-maker-dialog/post-maker-dialog.component';
import { TimeAgoPipe } from '../../pipes/time-ago.pipe';
import { TweetItemComponent } from './tweet-item.component';

describe('Tweet item component', () => {
  let component: TweetItemComponent;
  let fixture: ComponentFixture<TweetItemComponent>;
  let dialogSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule, MatDialogModule],
      declarations: [TweetItemComponent, TimeAgoPipe],
      providers: [
        { provide: FollowService, useClass: FollowServiceMock },
        { provide: TweetService, useClass: TweetServiceMock },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TweetItemComponent);
    component = fixture.componentInstance;
    dialogSpy = spyOn(component.dialog, 'open').and.returnValue({
      afterClosed: () => EMPTY,
    } as any);

    fixture.detectChanges();
    component.tweet = {
      text: 'random',
      hashtags: ['test'],
      id: '1',
      createdAt: new Date(),
      isQuoteRetweet: false,
      isRetweet: false,
      likes: ['1', '2'],
      comments: ['1', '2'],
      user: {
        userId: '1',
      },
      retweetUsers: ['1', '2'],
      quoteRetweets: ['1', '2'],
    };
  });

  it('should create the tweet item', () => {
    expect(TweetItemComponent).toBeTruthy();
  });

  it('should click quote button', () => {
    spyOn(component, 'onQuote');
    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Quote')
      )
      .triggerEventHandler('click', null);
    expect(component.onQuote).toHaveBeenCalled();
  });

  it('should open dialog onQuote function', () => {
    component.onQuote();

    expect(dialogSpy).toHaveBeenCalledWith(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isQuoteRetweet: true,
        originalTweetId: component.tweet?.id,
      },
    });
  });

  it('should open dialog onEdit function', () => {
    component.onEdit();

    expect(dialogSpy).toHaveBeenCalledWith(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isEdit: true,
        id: component.tweet?.id,
        text: component.tweet?.text,
        isQuoteRetweet: component.tweet?.isQuoteRetweet,
        originalTweetId: component.tweet?.originalTweetId,
      },
    });
  });

  it('should open dialog onViewLikes function', () => {
    component.onViewLikes();

    expect(dialogSpy).toHaveBeenCalledWith(LikersRetweetersDialogComponent, {
      width: '500px',
      data: {
        tweetId: component.tweet?.id,
        type: 'likes',
      },
    });
  });

  it('should open dialog onViewRetweeters function', () => {
    component.onViewRetweeters();
    expect(dialogSpy).toHaveBeenCalledWith(LikersRetweetersDialogComponent, {
      width: '500px',
      data: {
        tweetId: component.tweet?.id,
        type: 'retweeters',
      },
    });
  });
});
