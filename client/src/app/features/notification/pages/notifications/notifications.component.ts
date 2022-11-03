import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/core/services/notification.service';
import { UserStore } from 'src/app/core/store/user.store';
import { NotificationModel } from '../../../../core/models/notification.model';
import { PaginationModel } from '../../../../core/models/pagination.model';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
})
export class NotificationsComponent implements OnInit {
  pagination: PaginationModel = { pageNumber: 1 };

  notificationsByDate?: Map<string, NotificationModel[]>;
  originalOrder = (a: any, b: any) => 0;

  constructor(
    private userStore: UserStore,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.notificationService.notifications.subscribe((res) => {
      this.notificationsByDate = this.groupNotificationsByDate(res);
    });

    this.userStore.userAuth.subscribe((res) => {
      this.getNotifications();
    });
  }

  getNotifications(): void {
    this.notificationService.getNotifications(this.pagination).subscribe();
  }

  onScroll(): void {
    this.pagination.pageNumber++;
    this.getNotifications();
  }

  onNotificationClick(notification: NotificationModel): void {
    if (!notification.isRead)
      this.onNotificationMarkAsRead(notification.notificationId);

    if (
      notification.eventType === 'comment' ||
      notification.eventType === 'likeComment'
    ) {
      this.router.navigate(['/tweet', notification.tweetId], {
        queryParams: { comment: notification.commentId },
      });
    } else if (notification.eventType === 'follow') {
      this.router.navigate(['/profile', notification.eventTriggererId]);
    } else {
      this.router.navigate(['/tweet', notification.tweetId]);
    }
  }
  onNotificationDelete(notificationId: string): void {
    this.notificationService.deleteNotification(notificationId).subscribe();
  }

  onNotificationMarkAsRead(notificationId: string): void {
    this.notificationService.markAsRead(notificationId).subscribe();
  }

  groupNotificationsByDate(notifications: NotificationModel[]) {
    return notifications.reduce((r, a) => {
      const date = new Date(a.dateTime);
      const key = new Date(
        date.getFullYear(),
        date.getMonth(),
        date.getDate()
      ).toISOString();

      r.set(key, [...(r.get(key) || []), a]);
      return r;
    }, new Map<string, NotificationModel[]>());
  }
}