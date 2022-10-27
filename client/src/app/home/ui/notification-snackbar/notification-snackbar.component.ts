import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import {
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';
import { NotificationModel } from 'src/app/home/models/notification.model';

@Component({
  selector: 'app-notification-snackbar',
  templateUrl: './notification-snackbar.component.html',
  styleUrls: ['./notification-snackbar.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class NotificationSnackbarComponent implements OnInit {
  constructor(
    public snackbarRef: MatSnackBarRef<NotificationSnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: NotificationModel
  ) {}

  ngOnInit(): void {}

  onClose(): void {
    this.snackbarRef.dismiss();
  }
}
