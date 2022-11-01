import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UserModel } from 'src/app/home/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserStore {
  public readonly userAuth = new BehaviorSubject<UserModel>({});

  constructor() {}

  setUserAuth(user: UserModel) {
    this.userAuth.next(user);
  }

  clearUserAuth() {
    this.userAuth.next({});
  }
}
