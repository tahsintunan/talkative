import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';
import { FollowStore } from '../store/follow.store';

@Injectable({
  providedIn: 'root',
})
export class FollowService {
  apiUrl = this.env.apiUrl + 'api/Follow';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private followStore: FollowStore
  ) {}

  loadUserFollowings() {
    this.getUserFollowings().subscribe();
  }

  follow(followingId: string) {
    return this.http
      .post(this.apiUrl, { followingId })
      .pipe(tap(() => this.followStore.addToUserFollowings(followingId)));
  }

  unfollow(followingId: string) {
    return this.http
      .delete(this.apiUrl + '/' + followingId)
      .pipe(tap(() => this.followStore.removeFromUserFollowings(followingId)));
  }

  getUserFollowings() {
    return this.http
      .get<Record<string, boolean>>(this.apiUrl + '/following-id')
      .pipe(tap((res) => this.followStore.userFollowings.next(res)));
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
