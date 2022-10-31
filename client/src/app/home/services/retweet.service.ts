import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { TweetStore } from '../../shared/store/tweet.store';
import { PaginationModel } from '../models/pagination.model';
import { RetweetReqModel, TweetModel } from '../models/tweet.model';
import { UserModel } from '../models/user.model';
import { TweetService } from './tweet.service';

@Injectable({
  providedIn: 'root',
})
export class RetweetService {
  apiUrl = this.env.apiUrl + 'api/Retweet';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private tweetStore: TweetStore,
    private tweetService: TweetService,
    private router: Router
  ) {}

  createRetweet(retweet: RetweetReqModel) {
    return this.http.post<TweetModel>(this.apiUrl, retweet).pipe(
      tap((res) => {
        this.tweetService.addToUserTweets(res);
        this.tweetStore.updateTweetInTweetList(res.originalTweet!);
      })
    );
  }

  undoRetweet(originalTweetId: string) {
    return this.http.delete(this.apiUrl + '/' + originalTweetId).pipe(
      tap(() => {
        this.tweetService
          .getTweetById(originalTweetId)
          .subscribe((res) => this.tweetStore.updateTweetInTweetList(res));
      })
    );
  }

  deleteQuoteRetweet(tweetId: string, originalTweetId: string) {
    return this.http
      .delete(this.apiUrl + '/quote-retweet', {
        params: { tweetId, originalTweetId },
      })
      .pipe(
        tap(() => {
          if (!this.router.url.includes('/tweet/')) {
            this.tweetStore.removeTweetFromTweetList(tweetId);
            this.tweetService.getTweetById(originalTweetId).subscribe((res) => {
              if (res) this.tweetStore.updateTweetInTweetList(res);
            });
          }
        })
      );
  }

  getRetweeters(tweetId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl + '/retweeters/' + tweetId, {
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
}
