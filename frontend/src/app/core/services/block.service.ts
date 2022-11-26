import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';
import { BlockStore } from '../store/block.store';

@Injectable({
  providedIn: 'root',
})
export class BlockService {
  apiUrl = this.env.apiUrl + 'api/Block';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private blockStore: BlockStore
  ) {}

  loadUserBlockList() {
    this.getUserBlockList().subscribe();
  }

  blockUser(userId: string) {
    return this.http
      .post(this.apiUrl + '/' + userId, { userId })
      .pipe(tap(() => this.blockStore.addToUserBlockList(userId)));
  }

  unblockUser(userId: string) {
    return this.http
      .delete(this.apiUrl + '/' + userId)
      .pipe(tap(() => this.blockStore.removeFromUserBlockList(userId)));
  }

  getBlockList(userId: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.apiUrl, {
      params: { userId, ...pagination },
    });
  }

  getUserBlockList() {
    return this.http
      .get<Record<string, boolean>>(this.apiUrl + '/blocked-id')
      .pipe(tap((res) => this.blockStore.userBlockList.next(res)));
  }
}
