import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { FollowModel } from '../models/follow.model';

@Injectable({
  providedIn: 'root',
})
export class FollowService {
  apiUrl = this.env.apiUrl + 'api/Follow';

  private readonly userFollowersSubject = new BehaviorSubject<FollowModel[]>(
    []
  );
  private readonly userFollowingsSubject = new BehaviorSubject<FollowModel[]>(
    []
  );

  public readonly userFollowers = this.userFollowersSubject.asObservable();
  public readonly userFollowings = this.userFollowingsSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  init() {
    this.getUserFollowers();
    this.getUserFollowings();
  }

  followUser(followingId: string) {
    return this.http.post(this.apiUrl, { followingId }).pipe(
      tap(() => {
        this.init();
      })
    );
  }

  unfollowUser(followingId: string) {
    return this.http.delete(this.apiUrl + '/' + followingId).pipe(
      tap(() => {
        this.init();
      })
    );
  }

  getUserFollowers() {
    this.http.get<FollowModel[]>(this.apiUrl + '/follower').subscribe((res) => {
      this.userFollowersSubject.next(res);
    });
  }

  getUserFollowings() {
    this.http
      .get<FollowModel[]>(this.apiUrl + '/following')
      .subscribe((res) => {
        this.userFollowingsSubject.next(res);
      });
  }

  getFollowers(userId: string) {
    return this.http.get<FollowModel[]>(this.apiUrl + '/follower/' + userId);
  }

  getFollowings(userId: string) {
    return this.http.get<FollowModel[]>(this.apiUrl + '/following/' + userId);
  }
}
