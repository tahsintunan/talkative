import { HttpClient, HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { TweetStore } from 'src/app/core/store/tweet.store';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import { SearchSuggestionModel } from '../models/search.model';
import { TweetModel } from '../models/tweet.model';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  apiUrl = this.env.apiUrl + 'api/Search';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private tweetStore: TweetStore
  ) {}

  getSearchSuggestions(search: string, pagination: PaginationModel) {
    if (search.startsWith('#')) {
      return this.searchHashtagSuggestion(search, pagination);
    } else {
      return this.searchUserSuggestion(search, pagination);
    }
  }

  searchHashtagSuggestion(
    hashtag: string,
    pagination: PaginationModel
  ): Observable<SearchSuggestionModel[]> {
    return this.http
      .get<{ hashtags: string[] }>(
        this.apiUrl +
          '/hashtag/' +
          new HttpUrlEncodingCodec().encodeValue(hashtag),
        {
          params: { ...pagination },
        }
      )
      .pipe(
        map((res) =>
          res.hashtags.map((item) => ({
            type: 'hashtag',
            key: item,
            value: item,
          }))
        )
      );
  }

  searchUserSuggestion(
    username: string,
    pagination: PaginationModel
  ): Observable<SearchSuggestionModel[]> {
    return this.http
      .get<UserModel[]>(
        this.apiUrl +
          '/user/' +
          new HttpUrlEncodingCodec().encodeValue(username),
        {
          params: { ...pagination },
        }
      )
      .pipe(
        map((res) =>
          res.map((item) => ({
            type: 'user',
            key: item.userId!,
            value: item.username!,
          }))
        )
      );
  }

  getTweetsByHashtag(hashtag: string, pagination: PaginationModel) {
    return this.http
      .get<TweetModel[]>(
        this.apiUrl +
          '/tweet/' +
          new HttpUrlEncodingCodec().encodeValue(hashtag),
        {
          params: { ...pagination },
        }
      )
      .pipe(
        tap((res) =>
          this.tweetStore.addTweetsToTweetList(res, pagination.pageNumber)
        )
      );
  }

  getUsersByUsername(username: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(
      this.apiUrl + '/user/' + new HttpUrlEncodingCodec().encodeValue(username),
      {
        params: { ...pagination },
      }
    );
  }
}
