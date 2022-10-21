import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class FollowService {
  apiUrl = this.env.apiUrl + 'api/Follow';

  private readonly userFollowersSubject = new BehaviorSubject<UserModel[]>([]);
  private readonly userFollowingsSubject = new BehaviorSubject<UserModel[]>([]);

  public readonly userFollowers = this.userFollowersSubject.asObservable();
  public readonly userFollowings = this.userFollowingsSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  init() {
    this.getUserFollowers().subscribe();
    this.getUserFollowings().subscribe();
  }

  followUser(followingId: string) {
    return this.http
      .post(this.apiUrl, { followingId })
      .pipe(tap(() => this.init()));
  }

  unfollowUser(followingId: string) {
    return this.http
      .delete(this.apiUrl + '/' + followingId)
      .pipe(tap(() => this.init()));
  }

  getUserFollowers() {
    return this.http
      .get<UserModel[]>(this.apiUrl + '/follower')
      .pipe(tap((res) => this.userFollowersSubject.next(res)));
  }

  getUserFollowings() {
    return this.http
      .get<UserModel[]>(this.apiUrl + '/following')
      .pipe(tap((res) => this.userFollowingsSubject.next(res)));
  }

  getFollowers(userId: string) {
    return this.http.get<UserModel[]>(this.apiUrl + '/follower/' + userId);
  }

  getFollowings(userId: string) {
    return this.http.get<UserModel[]>(this.apiUrl + '/following/' + userId);
  }
}
