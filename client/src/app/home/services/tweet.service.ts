import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { TweetModel } from '../models/tweet.model';

@Injectable({
  providedIn: 'root',
})
export class TweetService {
  apiUrl = this.env.apiUrl + 'api/Tweet';

  private readonly tweetsSubject = new BehaviorSubject<TweetModel[]>([]);
  private readonly userTweetSubject = new BehaviorSubject<TweetModel[]>([]);

  public readonly tweets = this.tweetsSubject.asObservable();
  public readonly userTweets = this.userTweetSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  createTweet(tweet: TweetModel) {
    this.http.post<TweetModel>(this.apiUrl, tweet).subscribe((res) => {
      this.tweetsSubject.next([res, ...this.tweetsSubject.value]);

      if (res.user.id === this.userTweetSubject.value[0]?.user.id) {
        this.userTweetSubject.next([res, ...this.userTweetSubject.value]);
      }
    });
  }

  updateTweet(tweet: TweetModel) {
    this.http.put<TweetModel>(this.apiUrl, tweet).subscribe((res) => {
      this.tweetsSubject.next([
        ...this.tweetsSubject.value.map((x) => (x.id === res.id ? res : x)),
      ]);

      if (res.user.id === this.userTweetSubject.value[0]?.user.id) {
        this.userTweetSubject.next([
          ...this.userTweetSubject.value.map((x) =>
            x.id === res.id ? res : x
          ),
        ]);
      }
    });
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

  getTweets() {
    return this.http
      .get<TweetModel[]>(this.apiUrl + '/user/current-user')
      .subscribe((res) => {
        this.tweetsSubject.next(res);
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
}
