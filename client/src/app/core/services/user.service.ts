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
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = this.env.apiUrl + 'api/User';

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private cookie: CookieService,
    private userStore: UserStore,
    private authService: AuthService
  ) {}

  loadUserAuth() {
    const token = this.cookie.get('authorization');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);

        if (decodedToken.exp * 1000 < Date.now()) {
          this.authService.signout();
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
    const userId = this.userStore.userAuth.getValue()?.userId;
    return this.http.post(this.apiUrl + '/update-password', {
      userId,
      oldPassword,
      newPassword,
    });
  }

  updateCoverImage(image: File) {
    const formData = new FormData();
    formData.append('coverPicture', image);
    return this.http
      .patch<UserModel>(this.apiUrl + '/cover-picture', formData)
      .pipe(tap((res) => this.loadUserAuth()));
  }

  updateProfileImage(image: File) {
    const formData = new FormData();
    formData.append('profilePicture', image);
    return this.http
      .patch<UserModel>(this.apiUrl + '/profile-picture', formData)
      .pipe(tap((res) => this.loadUserAuth()));
  }

  getUserAnalytics(userId: string) {
    return this.http.get<UserAnalyticsModel>(this.apiUrl + '/count/' + userId);
  }

  getTopUsers() {
    return this.http.get<UserModel[]>(this.apiUrl + '/top-active-users');
  }
}
