import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { EnvService } from 'src/app/env.service';
import { NotificationService } from 'src/app/home/services/notification.service';
import { UserStore } from 'src/app/shared/store/user.store';
import { SignInReqModel } from '../models/signin.model';
import { SignUpReqModel } from '../models/signup.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  authUrl = this.envService.apiUrl + 'api/Auth';

  constructor(
    private http: HttpClient,
    private envService: EnvService,
    private cookieService: CookieService,
    private userStore: UserStore,
    private notificationService: NotificationService
  ) {}

  signup(data: SignUpReqModel) {
    return this.http.post(this.authUrl + '/signup', data, {
      withCredentials: true,
    });
  }

  signin(data: SignInReqModel) {
    return this.http.post(this.authUrl + '/login', data, {
      withCredentials: true,
    });
  }

  signout() {
    this.cookieService.delete('authorization');
    this.userStore.clearUserAuth();
    this.notificationService.stopConnection();
  }
}
