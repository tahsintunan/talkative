import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { EnvService } from 'src/app/env.service';
import { TweetModel } from '../models/tweet.model';

@Injectable({
  providedIn: 'root',
})
export class TweetService {
  apiUrl = this.env.apiUrl + 'api/Tweet/';

  constructor(
    private http: HttpClient,
    private cookie: CookieService,
    private env: EnvService
  ) {}

  createTweet(tweet: TweetModel) {
    const headers = new HttpHeaders();

    headers.set('Cookie', document.cookie);

    return this.http.post(this.apiUrl, tweet, {
      headers: headers,
      withCredentials: true,
    });
  }
}
