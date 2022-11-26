import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FollowStore {
  public readonly userFollowings = new BehaviorSubject<Record<string, boolean>>(
    {}
  );

  addToUserFollowings(followingId: string) {
    this.userFollowings.next({
      ...this.userFollowings.getValue(),
      [followingId]: true,
    });
  }

  removeFromUserFollowings(followingId: string) {
    this.userFollowings.next({
      ...this.userFollowings.getValue(),
      [followingId]: false,
    });
  }

  clearFollowings() {
    this.userFollowings.next({});
  }
}
