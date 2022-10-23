import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { RetweetReqModel, TweetModel } from '../models/tweet.model';
import { TweetService } from './tweet.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class RetweetService {
  apiUrl = this.env.apiUrl + 'api/Retweet';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private userServie: UserService,
    private tweetService: TweetService,
    private router: Router
  ) {}

  createRetweet(retweet: RetweetReqModel) {
    return this.http.post<TweetModel>(this.apiUrl, retweet).pipe(
      tap((res) => {
        this.tweetService.addToUserTweets(res);
        this.tweetService.refreshTweet(res.originalTweet!);
      })
    );
  }

  undoRetweet(originalTweetId: string) {
    return this.http.delete(this.apiUrl + '/' + originalTweetId).pipe(
      tap(() => {
        this.tweetService.getTweetById(originalTweetId).subscribe((res) => {
          this.tweetService.refreshTweet(res);
        });

        this.userServie.userAuth.subscribe((user) => {
          if (this.router.url.includes('profile')) {
            const activeProfileId = this.router.url.split('/profile/')[1];
            if (user.userId === activeProfileId)
              this.tweetService.userTweets.subscribe((tweets) => {
                tweets.forEach((tweet) => {
                  if (
                    tweet.user?.userId === user?.userId &&
                    tweet.isRetweet &&
                    tweet.originalTweet?.id === originalTweetId
                  )
                    this.tweetService.removeTweetFromList(tweet.id);
                });
              });
          }

          if (this.router.url.includes('feed')) {
            this.tweetService.feedTweets.subscribe((tweets) => {
              tweets.forEach((tweet) => {
                if (
                  tweet.isRetweet &&
                  tweet.originalTweet?.id === originalTweetId
                )
                  this.tweetService.removeTweetFromList(tweet.id);
              });
            });
          }
        });
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
          this.tweetService.removeTweetFromList(tweetId);
          this.tweetService.getTweetById(originalTweetId).subscribe((res) => {
            this.tweetService.refreshTweet(res);
          });
        })
      );
  }
}
