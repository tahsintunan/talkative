import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { TweetStore } from '../../shared/store/tweet.store';
import { RetweetReqModel, TweetModel } from '../models/tweet.model';
import { TweetService } from './tweet.service';

@Injectable({
  providedIn: 'root',
})
export class RetweetService {
  apiUrl = this.env.apiUrl + 'api/Retweet';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private storeService: TweetStore,
    private tweetService: TweetService,
    private router: Router
  ) {}

  createRetweet(retweet: RetweetReqModel) {
    return this.http.post<TweetModel>(this.apiUrl, retweet).pipe(
      tap((res) => {
        this.tweetService.addToUserTweets(res);
        this.storeService.updateTweetInTweetList(res.originalTweet!);
      })
    );
  }

  undoRetweet(originalTweetId: string) {
    return this.http.delete(this.apiUrl + '/' + originalTweetId).pipe(
      tap(() => {
        this.tweetService
          .getTweetById(originalTweetId)
          .subscribe((res) => this.storeService.updateTweetInTweetList(res));
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
            this.storeService.removeTweetFromTweetList(tweetId);
            this.tweetService.getTweetById(originalTweetId).subscribe((res) => {
              if (res) this.storeService.updateTweetInTweetList(res);
            });
          }
        })
      );
  }
}
