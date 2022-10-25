import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { PaginationModel } from '../models/pagination.model';
import {
  TweetCreateReqModel,
  TweetModel,
  TweetUpdateReqModel,
} from '../models/tweet.model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class TweetService {
  apiUrl = this.env.apiUrl + 'api/Tweet';

  private readonly feedTweetsSubject = new BehaviorSubject<TweetModel[]>([]);
  private readonly userTweetSubject = new BehaviorSubject<TweetModel[]>([]);

  public readonly feedTweets = this.feedTweetsSubject.asObservable();
  public readonly userTweets = this.userTweetSubject.asObservable();

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private userService: UserService,
    private router: Router
  ) {}

  createTweet(tweet: TweetCreateReqModel) {
    return this.http.post<TweetModel>(this.apiUrl, tweet).pipe(
      tap((res) => {
        this.addToUserTweets(res);
      })
    );
  }

  updateTweet(tweet: TweetUpdateReqModel) {
    return this.http
      .put<TweetModel>(this.apiUrl, tweet)
      .pipe(tap((res) => this.refreshTweet(res)));
  }

  deleteTweet(tweetId: string) {
    return this.http
      .delete(this.apiUrl + '/' + tweetId)
      .pipe(tap(() => this.removeTweetFromList(tweetId)));
  }

  getTweetById(id: string) {
    return this.http.get<TweetModel>(this.apiUrl + '/' + id);
  }

  getTweets(pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/feed', { params: { ...pagination } })
      .pipe(
        tap((res) => {
          res = this.filterNullRetweets(res);
          if (pagination?.pageNumber == 1) this.feedTweetsSubject.next(res);
          else
            this.feedTweetsSubject.next([
              ...this.feedTweetsSubject.getValue(),
              ...res,
            ]);
        })
      );
  }

  getUserTweets(userId: string, pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/user/' + userId, {
        params: { ...pagination },
      })
      .pipe(
        tap((res) => {
          res = this.filterNullRetweets(res);
          if (pagination?.pageNumber == 1) this.userTweetSubject.next(res);
          else
            this.userTweetSubject.next([
              ...this.userTweetSubject.getValue(),
              ...res,
            ]);
        })
      );
  }

  likeTweet(tweetId: string, isLiked: boolean) {
    return this.http
      .put<TweetModel>(this.apiUrl + '/like', {
        tweetId,
        isLiked,
      })
      .pipe(tap((res) => this.refreshTweet(res)));
  }

  addToUserTweets(tweet: TweetModel) {
    this.userService.userAuth.subscribe((user) => {
      const activeProfileId = this.router.url.split('/profile/')[1];
      if (user.userId === activeProfileId && tweet.user.userId === user.userId)
        this.userTweetSubject.next([
          tweet,
          ...this.userTweetSubject.getValue(),
        ]);
    });
  }

  refreshTweet(tweet: TweetModel) {
    if (this.router.url.includes('feed'))
      this.feedTweetsSubject.next(
        this.refreshTweetInList(tweet, this.feedTweetsSubject.getValue())
      );

    if (this.router.url.includes('profile'))
      this.userTweetSubject.next(
        this.refreshTweetInList(tweet, this.userTweetSubject.getValue())
      );
  }

  refreshTweetInList(tweet: TweetModel, tweets: TweetModel[]) {
    return tweets.map((x) => {
      if (x.id === tweet.id) return tweet;
      else if (x.originalTweet?.id === tweet.id)
        return { ...x, originalTweet: tweet };
      else return x;
    });
  }

  removeTweetFromList(tweetId: string) {
    if (this.router.url.includes('feed')) {
      this.feedTweetsSubject.next(
        this.filterDeletedTweet(tweetId, this.feedTweetsSubject.getValue())
      );
    }

    if (this.router.url.includes('profile'))
      this.userTweetSubject.next(
        this.filterDeletedTweet(tweetId, this.userTweetSubject.getValue())
      );
  }

  filterDeletedTweet(tweetId: string, tweets: TweetModel[]) {
    return this.filterNullRetweets(
      tweets
        .filter(
          (x) =>
            x.id !== tweetId || (x.isRetweet && x.originalTweet?.id !== tweetId)
        )
        .map((x) =>
          x.originalTweet?.id === tweetId
            ? { ...x, originalTweet: undefined }
            : x
        )
    );
  }

  filterNullRetweets(tweets: TweetModel[]) {
    return tweets.filter((x) => !(x.isRetweet && !x.originalTweet));
  }
}
