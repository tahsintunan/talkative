import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { tap } from 'rxjs';
import { UserStore } from 'src/app/core/store/user.store';
import { EnvService } from 'src/app/shared/services/env.service';
import {
  UserAnalyticsModel,
  UserModel,
  UserUpdateReqModel,
} from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = this.env.apiUrl + 'api/User';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private cookie: CookieService,
    private userStore: UserStore
  ) {}

  loadUserAuth() {
    const token = this.cookie.get('authorization');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);

        if (decodedToken.exp * 1000 < Date.now()) {
          this.cookie.delete('authorization');
          this.userStore.clearUserAuth();
        } else if (decodedToken.user_id) {
          this.getUser(decodedToken.user_id).subscribe((res) => {
            this.userStore.setUserAuth(res);
          });
        }
      } catch (error) {
        console.log(error);
      }
    }
  }

  getUser(userId: string) {
    return this.http.get<UserModel>(this.apiUrl + '/' + userId);
  }

  updateProfile(user: UserUpdateReqModel) {
    return this.http
      .put<UserModel>(this.apiUrl, user)
      .pipe(tap((res) => this.loadUserAuth()));
  }

  updatePassword(oldPassword: string, newPassword: string) {
    return this.http.post(this.apiUrl + '/update-password', {
      oldPassword,
      newPassword,
    });
  }

  getUserAnalytics(userId: string) {
    return this.http.get<UserAnalyticsModel>(this.apiUrl + '/count/' + userId);
  }

  getTopUsers() {
    return this.http.get<UserModel[]>(this.apiUrl + '/top-active-users');
  }
}
