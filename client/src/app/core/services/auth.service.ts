import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { SignInReqModel } from 'src/app/core/models/signin.model';
import { SignUpReqModel } from 'src/app/core/models/signup.model';
import { UserStore } from 'src/app/core/store/user.store';
import { EnvService } from 'src/app/shared/services/env.service';
import { BlockStore } from '../store/block.store';
import { FollowStore } from '../store/follow.store';
import { NotificationStore } from '../store/notification.store';
import { TweetStore } from '../store/tweet.store';
import { NotificationService } from './notification.service';

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
    private notificationService: NotificationService,
    private followStore: FollowStore,
    private blockStore: BlockStore,
    private tweetStore: TweetStore,
    private notificationStore: NotificationStore
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
    this.http.post(this.authUrl + '/logout', null).subscribe();
    this.cookieService.delete('authorization');
    this.userStore.clearUserAuth();
    this.followStore.clearFollowings();
    this.blockStore.clearBlockList();
    this.tweetStore.clearTweetList();
    this.notificationStore.clearNotifications();
    this.notificationService.stopConnection();
  }

  forgotPassword(email: string) {
    return this.http.post(this.authUrl + '/forget-password', { email });
  }
}
