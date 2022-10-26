import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { TweetStore } from '../../shared/store/tweet.store';
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

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private storeService: TweetStore,
    private userService: UserService,
    private router: Router
  ) {}

  createTweet(tweet: TweetCreateReqModel) {
    return this.http
      .post<TweetModel>(this.apiUrl, tweet)
      .pipe(tap((res) => this.addToUserTweets(res)));
  }

  updateTweet(tweet: TweetUpdateReqModel) {
    return this.http
      .put<TweetModel>(this.apiUrl, tweet)
      .pipe(tap((res) => this.storeService.updateTweetInTweetList(res)));
  }

  deleteTweet(tweetId: string) {
    return this.http
      .delete(this.apiUrl + '/' + tweetId)
      .pipe(tap(() => this.storeService.removeTweetFromTweetList(tweetId)));
  }

  getTweetById(id: string) {
    return this.http.get<TweetModel>(this.apiUrl + '/' + id);
  }

  getTweets(pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/feed', { params: { ...pagination } })
      .pipe(
        tap((res) =>
          this.storeService.addTweetsToTweetList(res, pagination.pageNumber)
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
          this.storeService.addTweetsToTweetList(res, pagination.pageNumber)
        )
      );
  }

  likeTweet(tweetId: string, isLiked: boolean) {
    return this.http
      .put<TweetModel>(this.apiUrl + '/like', {
        tweetId,
        isLiked,
      })
      .pipe(tap((res) => this.storeService.updateTweetInTweetList(res)));
  }

  addToUserTweets(tweet: TweetModel) {
    this.userService.userAuth.subscribe((user) => {
      const activeProfileId = this.router.url.split('/profile/')[1];
      if (user.userId === activeProfileId && tweet.user.userId === user.userId)
        this.storeService.addTweetToTweetList(tweet);
    });
  }
}
