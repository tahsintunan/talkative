import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserStore } from 'src/app/core/store/user.store';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import {
  TrendingHashtagModel,
  TweetCreateReqModel,
  TweetModel,
  TweetUpdateReqModel,
} from '../models/tweet.model';
import { UserModel } from '../models/user.model';
import { TweetStore } from '../store/tweet.store';

@Injectable({
  providedIn: 'root',
})
export class TweetService {
  apiUrl = this.env.apiUrl + 'api/Tweet';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private tweetStore: TweetStore,
    private userStore: UserStore,
    private router: Router
  ) {}

  createTweet(tweet: TweetCreateReqModel) {
    return this.http
      .post<TweetModel>(this.apiUrl, tweet)
      .pipe(tap((res) => this.addToTweets(res)));
  }

  updateTweet(tweet: TweetUpdateReqModel) {
    return this.http
      .put<TweetModel>(this.apiUrl, tweet)
      .pipe(
        tap(
          (res) =>
            !this.router.url.includes('/tweet/') &&
            this.tweetStore.updateTweetInTweetList(res)
        )
      );
  }

  deleteTweet(tweetId: string) {
    return this.http
      .delete(this.apiUrl + '/' + tweetId)
      .pipe(
        tap(
          () =>
            !this.router.url.includes('/tweet/') &&
            this.tweetStore.removeTweetFromTweetList(tweetId)
        )
      );
  }

  getTweetById(id: string) {
    return this.http.get<TweetModel>(this.apiUrl + '/' + id);
  }

  getTweets(pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/feed', { params: { ...pagination } })
      .pipe(
        tap((res) =>
          this.tweetStore.addTweetsToTweetList(res, pagination.pageNumber)
        )
      );
  }

  getUserTweets(userId: string, pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/user/' + userId, {
        params: { ...pagination },
      })
      .pipe(
        tap((res) =>
          this.tweetStore.addTweetsToTweetList(res, pagination.pageNumber)
        )
      );
  }

  likeTweet(tweetId: string, isLiked: boolean) {
    return this.http
      .put<TweetModel>(this.apiUrl + '/like', {
        tweetId,
        isLiked,
      })
      .pipe(tap((res) => this.tweetStore.updateTweetInTweetList(res)));
  }

  getRetweeters(tweetId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl + '/retweeters/' + tweetId, {
      params: { ...pagination },
    });
  }

  getLikedUsers(tweetId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl + '/like/' + tweetId, {
      params: { ...pagination },
    });
  }

  getQuotes(tweetId: string, pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/quote-retweet/' + tweetId, {
        params: { ...pagination },
      })
      .pipe(
        tap((res) =>
          this.tweetStore.addTweetsToTweetList(res, pagination.pageNumber)
        )
      );
  }

  getTrendingHashtags() {
    return this.http.get<TrendingHashtagModel[]>(
      this.apiUrl + '/trending-hashtags'
    );
  }

  addToTweets(tweet: TweetModel) {
    const userAuth = this.userStore.userAuth.getValue();

    const isFeed = this.router.url.endsWith('/tweet');
    const isAuthUser =
      this.router.url.split('/profile/')[1] === userAuth.userId;

    if (isFeed || (isAuthUser && tweet.user.userId === userAuth.userId))
      this.tweetStore.addTweetToTweetList(tweet);
  }
}
