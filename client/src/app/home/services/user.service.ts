import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { UserAnalyticsModel, UserModel, UserUpdateReqModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = this.env.apiUrl + 'api/User';

  private readonly userAuthSubject = new BehaviorSubject<UserModel>({});

  public readonly userAuth = this.userAuthSubject.asObservable();

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private cookie: CookieService
  ) {}

  loadUserAuth() {
    const token = this.cookie.get('authorization');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);

        if (decodedToken.exp * 1000 < Date.now()) {
          this.cookie.delete('authorization');
          this.userAuthSubject.next({});
        } else if (decodedToken.user_id) {
          this.getUser(decodedToken.user_id).subscribe((res) => {
            this.userAuthSubject.next(res);
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
}
