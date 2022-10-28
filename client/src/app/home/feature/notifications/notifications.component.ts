import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationModel } from '../../models/notification.model';
import { PaginationModel } from '../../models/pagination.model';
import { NotificationService } from '../../services/notification.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
})
export class NotificationsComponent implements OnInit {
  pagination: PaginationModel = { pageNumber: 1 };

  notificationsByDate?: Record<string, NotificationModel[]>;

  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.notificationService.notifications.subscribe((res) => {
      this.notificationsByDate =
        this.notificationService.groupNotificationsByDate(res);
    });

    this.userService.userAuth.subscribe((res) => {
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
      this.router.navigate(['/home/tweet', notification.tweetId], {
        queryParams: { comment: notification.commentId },
      });
    } else if (notification.eventType === 'follow') {
      this.router.navigate(['/home/profile', notification.eventTriggererId]);
    } else {
      this.router.navigate(['/home/tweet', notification.tweetId]);
    }
  }
  onNotificationDelete(notificationId: string): void {
    this.notificationService.deleteNotification(notificationId).subscribe();
  }

  onNotificationMarkAsRead(notificationId: string): void {
    this.notificationService.markAsRead(notificationId).subscribe();
  }
}
