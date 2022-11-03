import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { FollowStore } from './follow.store';

@Injectable({
  providedIn: 'root',
})
export class BlockStore {
  public readonly userBlockList = new BehaviorSubject<Record<string, boolean>>(
    {}
  );

  constructor(private followStore: FollowStore) {}

  addToUserBlockList(blockedId: string) {
    this.userBlockList.next({
      ...this.userBlockList.getValue(),
      [blockedId]: true,
    });
    this.followStore.removeFromUserFollowings(blockedId);
  }

  removeFromUserBlockList(blockedId: string) {
    this.userBlockList.next({
      ...this.userBlockList.getValue(),
      [blockedId]: false,
    });
  }
}
