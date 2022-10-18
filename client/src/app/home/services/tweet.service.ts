import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
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
    this.http.post<TweetModel>(this.apiUrl, tweet).subscribe((res) => {
      this.feedTweetsSubject.next([res, ...this.feedTweetsSubject.value]);

      this.addToUserTweets(res);
    });
  }

  retweet(tweet: TweetRetweetReqModel) {
    this.http
      .post<TweetModel>(this.apiUrl + '/retweet', tweet)
      .subscribe((res) => {
        this.feedTweetsSubject.next([res, ...this.feedTweetsSubject.value]);

        this.addToUserTweets(res);
      });
  }

  updateTweet(tweet: TweetUpdateReqModel) {
    this.http.put<TweetModel>(this.apiUrl, tweet).subscribe((res) => {
      this.feedTweetsSubject.next([
        ...this.feedTweetsSubject.value.map((x) => (x.id === res.id ? res : x)),
      ]);

      this.userTweetSubject.next([
        ...this.userTweetSubject.value.map((x) => (x.id === res.id ? res : x)),
      ]);
    });
  }

  deleteTweet(tweetId: string) {
    this.http.delete(this.apiUrl + '/' + tweetId).subscribe((res) => {
      this.feedTweetsSubject.next(
        this.feedTweetsSubject.value.filter((x) => x.id !== tweetId)
      );

      this.userTweetSubject.next(
        this.userTweetSubject.value.filter((x) => x.id !== tweetId)
      );
    });
  }

  getTweets() {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/user/current-user')
      .subscribe((res) => {
        this.feedTweetsSubject.next(res);
      });
  }

  getTweetById(id: string) {
    return this.http.get<TweetModel>(this.apiUrl + '/' + id);
  }

  getUserTweets(userId: string) {
    this.http
      .get<TweetModel[]>(this.apiUrl + '/user/' + userId)
      .subscribe((res) => {
        this.userTweetSubject.next(res);
      });
  }

  likeTweet(tweetId: string, isLiked: boolean) {
    this.http
      .put<TweetModel>(this.apiUrl + '/like', { tweetId, isLiked })
      .subscribe((res) => {});
  }

  private addToUserTweets(tweet: TweetModel) {
    const currentUserId = this.router.url.split('/profile/')[1];

    if (tweet.user?.id === currentUserId) {
      this.userTweetSubject.next([tweet, ...this.userTweetSubject.value]);
    }
  }
}
