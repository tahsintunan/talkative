import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { EnvService } from 'src/app/shared/services/env.service';
import { RetweetReqModel, TweetModel } from '../models/tweet.model';
import { TweetStore } from '../store/tweet.store';
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
        this.tweetService.addToTweets(res);
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
}
