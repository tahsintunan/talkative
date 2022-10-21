import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class BlockService {
  apiUrl = this.env.apiUrl + 'api/Block';

  private readonly userBlockListSubject = new BehaviorSubject<UserModel[]>([]);

  public readonly userBlockList = this.userBlockListSubject.asObservable();

  constructor(private http: HttpClient, private env: EnvService) {}

  init() {
    this.getUserBlockList().subscribe();
  }

  blockUser(userId: string) {
    return this.http.post(this.apiUrl + '/' + userId, { userId });
  }

  unblockUser(userId: string) {
    return this.http.delete(this.apiUrl + '/' + userId);
  }

  getUserBlockList() {
    return this.http
      .get<UserModel[]>(this.apiUrl)
      .pipe(tap((res) => this.userBlockListSubject.next(res)));
  }
}
