import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class BlockService {
  apiUrl = this.env.apiUrl + 'api/Block';

  private readonly userBlockListSubject = new BehaviorSubject<
    Record<string, boolean>
  >({});
  public readonly userBlockList = this.userBlockListSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  loadUserBlockList() {
    this.getUserBlockList().subscribe();
  }

  blockUser(userId: string) {
    return this.http.post(this.apiUrl + '/' + userId, { userId });
  }

  unblockUser(userId: string) {
    return this.http.delete(this.apiUrl + '/' + userId);
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
