import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { TweetModel } from '../models/tweet.model';

@Injectable({
  providedIn: 'root',
})
export class TweetService {
  apiUrl = this.env.apiUrl + 'api/Tweet';

  private tweetsSubject = new BehaviorSubject<TweetModel[]>([]);
  private userTweetSubject = new BehaviorSubject<TweetModel[]>([]);

  public tweets = this.tweetsSubject.asObservable();
  public userTweets = this.userTweetSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  createTweet(tweet: TweetModel) {
    return this.http.post(this.apiUrl, tweet);
  }

  updateTweet(tweet: TweetModel) {
    return this.http.put(this.apiUrl, tweet);
  }

  deleteTweet(tweetId: string) {
    this.http.delete(this.apiUrl + '/' + tweetId).subscribe((res) => {
      this.tweetsSubject.next(
        this.tweetsSubject.value.filter((x) => x.id !== tweetId)
      );

      this.userTweetSubject.next(
        this.userTweetSubject.value.filter((x) => x.id !== tweetId)
      );
    });
  }

  getFeedTweets(): Observable<TweetModel[]> {
    return this.http.get<TweetModel[]>(this.apiUrl + '/user/current-user').pipe(
      tap((res) => {
        this.tweetsSubject.next(res);
      })
    );
  }

  getUserTweets(userId: string): Observable<TweetModel[]> {
    return this.http.get<TweetModel[]>(this.apiUrl + '/user/' + userId).pipe(
      tap((res) => {
        this.userTweetSubject.next(res);
      })
    );
  }
}
