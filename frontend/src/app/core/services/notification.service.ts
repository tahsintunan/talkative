import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { tap } from 'rxjs';
import { NotificationSnackbarComponent } from 'src/app/features/notification/ui/notification-snackbar/notification-snackbar.component';
import { EnvService } from '../../shared/services/env.service';
import { NotificationModel } from '../models/notification.model';
import { PaginationModel } from '../models/pagination.model';
import { NotificationStore } from '../store/notification.store';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  apiUrl = this.envService.apiUrl + 'api/Notification';
  notificationUrl = this.envService.apiUrl + 'notificationhub';

  private readonly connection = new signalR.HubConnectionBuilder()
    .withUrl(this.notificationUrl, {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .build();

  private notificationAudio = new Audio('assets/audios/notification.mp3');

  constructor(
    private http: HttpClient,
    private envService: EnvService,
    private notificationStore: NotificationStore,
    private snackBar: MatSnackBar
  ) {}

  initConnection() {
    this.connection.on('GetNotification', (notification: NotificationModel) => {
      if (this.notificationStore.addNotificationToList(notification))
        this.triggerNotification(notification);
    });

    if (this.connection.state === signalR.HubConnectionState.Disconnected)
      this.startConnection();
  }

  async startConnection() {
    try {
      await this.connection.start();
      console.log('connection started');
    } catch (err) {
      console.log(err);
      setTimeout(() => {
        this.startConnection();
      }, 2500);
    }
  }

  async stopConnection() {
    try {
      await this.connection.stop();
      console.log('connection stopped');
    } catch (err) {
      console.log(err);
    }
  }

  loadNotifications() {
    this.getNotifications({ pageNumber: 1 }).subscribe();
  }

  getNotifications(pagination: PaginationModel) {
    return this.http
      .get<NotificationModel[]>(this.apiUrl, {
        params: {
          ...pagination,
        },
      })
      .pipe(
        tap((res) => {
          res.length > 0 &&
            this.notificationStore.addNotificationsToList(
              res,
              pagination.pageNumber
            );
        })
      );
  }

  deleteNotification(notificationId: string) {
    return this.http
      .delete(this.apiUrl + '/' + notificationId)
      .pipe(
        tap(() =>
          this.notificationStore.removeNotificationFromList(notificationId)
        )
      );
  }

  markAllAsRead() {
    return this.http
      .patch(this.apiUrl + '/read-all', null)
      .pipe(tap(() => this.notificationStore.markAllAsRead()));
  }

  markAsRead(notificationId: string) {
    return this.http
      .patch(this.apiUrl + '/' + notificationId, { notificationId })
      .pipe(tap(() => this.notificationStore.markAsRead(notificationId)));
  }

  triggerNotification(notification: NotificationModel) {
    this.playNotificationSound();
    this.showNotificationPopup(notification);
  }

  showNotificationPopup(notification: NotificationModel) {
    this.snackBar.openFromComponent(NotificationSnackbarComponent, {
      duration: 10000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      data: notification,
    });
  }

  async playNotificationSound() {
    this.notificationAudio.volume = 0.5;
    try {
      await this.notificationAudio.play();
    } catch (error) {}
  }
}
