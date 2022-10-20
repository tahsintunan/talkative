import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import {
  TweetCreateReqModel,
  TweetModel,
  TweetRetweetReqModel,
  TweetUpdateReqModel,
} from '../models/tweet.model';

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
    private router: Router
  ) {}

  createTweet(tweet: TweetCreateReqModel) {
    return this.http.post<TweetModel>(this.apiUrl, tweet).pipe(
      tap((res) => {
        this.feedTweetsSubject.next([res, ...this.feedTweetsSubject.value]);

        this.addToUserTweets(res);
      })
    );
  }

  retweet(tweet: TweetRetweetReqModel) {
    return this.http.post<TweetModel>(this.apiUrl + '/retweet', tweet).pipe(
      tap((res) => {
        this.feedTweetsSubject.next([res, ...this.feedTweetsSubject.value]);

        this.addToUserTweets(res);
      })
    );
  }

  updateTweet(tweet: TweetUpdateReqModel) {
    return this.http.put<TweetModel>(this.apiUrl, tweet).pipe(
      tap((res) => {
        this.feedTweetsSubject.next([
          ...this.feedTweetsSubject.value.map((x) =>
            x.id === res.id ? res : x
          ),
        ]);

        this.userTweetSubject.next([
          ...this.userTweetSubject.value.map((x) =>
            x.id === res.id ? res : x
          ),
        ]);
      })
    );
  }

  deleteTweet(tweetId: string) {
    return this.http.delete(this.apiUrl + '/' + tweetId).pipe(
      tap(() => {
        this.feedTweetsSubject.next(
          this.feedTweetsSubject.value.filter((x) => x.id !== tweetId)
        );

        this.userTweetSubject.next(
          this.userTweetSubject.value.filter((x) => x.id !== tweetId)
        );
      })
    );
  }

  getTweets() {
    return this.http.get<TweetModel[]>(this.apiUrl + '/user/current-user').pipe(
      tap((res) => {
        this.feedTweetsSubject.next(res);
      })
    );
  }

  getTweetById(id: string) {
    return this.http.get<TweetModel>(this.apiUrl + '/' + id);
  }

  getUserTweets(userId: string) {
    return this.http.get<TweetModel[]>(this.apiUrl + '/user/' + userId).pipe(
      tap((res) => {
        this.userTweetSubject.next(res);
      })
    );
  }

  likeTweet(tweetId: string, isLiked: boolean) {
    return this.http.put<TweetModel>(this.apiUrl + '/like', {
      tweetId,
      isLiked,
    });
  }

  private addToUserTweets(tweet: TweetModel) {
    const currentUserId = this.router.url.split('/profile/')[1];

    if (tweet.user?.userId === currentUserId) {
      this.userTweetSubject.next([tweet, ...this.userTweetSubject.value]);
    }
  }
}
