import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';
import { FollowService } from './follow.service';

@Injectable({
  providedIn: 'root',
})
export class BlockService {
  apiUrl = this.env.apiUrl + 'api/Block';

  private readonly userBlockListSubject = new BehaviorSubject<
    Record<string, boolean>
  >({});
  public readonly userBlockList = this.userBlockListSubject.asObservable();

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private followService: FollowService
  ) {}

  loadUserBlockList() {
    this.getUserBlockList().subscribe();
  }

  addToUserBlockList(blockedId: string) {
    this.userBlockListSubject.next({
      ...this.userBlockListSubject.getValue(),
      [blockedId]: true,
    });
    this.followService.removeFromUserFollowings(blockedId);
  }

  removeFromUserBlockList(blockedId: string) {
    this.userBlockListSubject.next({
      ...this.userBlockListSubject.getValue(),
      [blockedId]: false,
    });
  }

  blockUser(userId: string) {
    return this.http
      .post(this.apiUrl + '/' + userId, { userId })
      .pipe(tap(() => this.addToUserBlockList(userId)));
  }

  unblockUser(userId: string) {
    return this.http
      .delete(this.apiUrl + '/' + userId)
      .pipe(tap(() => this.removeFromUserBlockList(userId)));
  }

  getBlockList(userId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl, {
      params: { userId, ...pagination },
    });
  }

  getUserBlockList() {
    return this.http
      .get<Record<string, boolean>>(this.apiUrl + '/blocked-id')
      .pipe(tap((res) => this.userBlockListSubject.next(res)));
  }
}
