import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import {
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { NotificationModel } from 'src/app/core/models/notification.model';
import { NotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-notification-snackbar',
  templateUrl: './notification-snackbar.component.html',
  styleUrls: ['./notification-snackbar.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class NotificationSnackbarComponent implements OnInit {
  constructor(
    private router: Router,
    private notificationService: NotificationService,
    public snackbarRef: MatSnackBarRef<NotificationSnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: NotificationModel
  ) {}

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  onClose(): void {
    this.snackbarRef.dismiss();
  }

  onClick(): void {
    this.notificationService.markAsRead(this.data.notificationId).subscribe();

    if (
      this.data.eventType === 'comment' ||
      this.data.eventType === 'likeComment'
    ) {
      this.router.navigate(['/tweet', this.data.tweetId], {
        queryParams: { comment: this.data.commentId },
      });
    } else if (this.data.eventType === 'follow') {
      this.router.navigate(['/profile', this.data.eventTriggererId]);
    } else {
      this.router.navigate(['/tweet', this.data.tweetId]);
    }

    this.onClose();
  }
}
