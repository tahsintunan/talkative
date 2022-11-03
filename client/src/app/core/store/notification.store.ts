import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotificationModel } from '../models/notification.model';
import { UserStore } from './user.store';

@Injectable({
  providedIn: 'root',
})
export class NotificationStore {
  public readonly notifications = new BehaviorSubject<NotificationModel[]>([]);

  constructor(private userStore: UserStore) {}

  addNotificationToList(notification: NotificationModel) {
    const userAuth = this.userStore.userAuth.getValue();
    if (
      notification.eventTriggererId !== userAuth?.userId &&
      notification.notificationReceiverId === userAuth?.userId
    ) {
      this.notifications.next([notification, ...this.notifications.getValue()]);
      return true;
    }

    return false;
  }

  addNotificationsToList(
    notifications: NotificationModel[],
    pageNumber: number
  ) {
    notifications = this.filterPersonalNotifications(notifications);
    if (pageNumber === 1) this.notifications.next(notifications);
    else
      this.notifications.next([
        ...this.notifications.getValue(),
        ...notifications,
      ]);
  }

  removeNotificationFromList(notificationId: string) {
    const notifications = this.notifications.getValue();
    const index = notifications.findIndex(
      (x) => x.notificationId === notificationId
    );

    if (index !== -1) {
      notifications.splice(index, 1);
      this.notifications.next(notifications);
    }
  }

  markAsRead(notificationId: string) {
    const notifications = this.notifications.getValue();
    const index = notifications.findIndex(
      (x) => x.notificationId === notificationId
    );

    if (index !== -1) {
      notifications[index].isRead = true;
      this.notifications.next(notifications);
    }
  }

  filterPersonalNotifications(notifications: NotificationModel[]) {
    const userAuth = this.userStore.userAuth.getValue();
    return notifications.filter(
      (notification) =>
        notification.eventTriggererId !== userAuth?.userId &&
        notification.notificationReceiverId === userAuth?.userId
    );
  }
}
