import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class FollowService {
  apiUrl = this.env.apiUrl + 'api/Follow';

  private readonly userFollowingsSubject = new BehaviorSubject<string[]>([]);

  public readonly userFollowings = this.userFollowingsSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  loadUserFollow() {
    this.getUserFollowings().subscribe();
  }

  followUser(followingId: string) {
    return this.http
      .post(this.apiUrl, { followingId })
      .pipe(tap(() => this.loadUserFollow()));
  }

  unfollowUser(followingId: string) {
    return this.http
      .delete(this.apiUrl + '/' + followingId)
      .pipe(tap(() => this.loadUserFollow()));
  }

  getUserFollowings() {
    return this.http
      .get<string[]>(this.apiUrl + '/following-id')
      .pipe(tap((res) => this.userFollowingsSubject.next(res)));
  }

  getFollowers(userId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl + '/follower/' + userId, {
      params: { ...pagination },
    });
  }

  getFollowings(userId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl + '/following/' + userId, {
      params: { ...pagination },
    });
  }
}
