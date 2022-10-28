import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { NotificationModel } from 'src/app/home/models/notification.model';

@Component({
  selector: 'app-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.css'],
})
export class NotificationItemComponent implements OnInit {
  @Input() data?: NotificationModel;
  @Output() onClick = new EventEmitter<NotificationModel>();
  @Output() onMarkAsRead = new EventEmitter();
  @Output() onDelete = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}
}
