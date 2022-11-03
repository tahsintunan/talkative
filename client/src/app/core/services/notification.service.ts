import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, tap } from 'rxjs';
import { UserStore } from 'src/app/core/store/user.store';
import { NotificationSnackbarComponent } from 'src/app/features/notification/ui/notification-snackbar/notification-snackbar.component';
import { EnvService } from '../../shared/services/env.service';
import { NotificationModel } from '../models/notification.model';
import { PaginationModel } from '../models/pagination.model';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private readonly notificationsSubject = new BehaviorSubject<
    NotificationModel[]
  >([]);
  public readonly notifications = this.notificationsSubject.asObservable();

  apiUrl = this.envService.apiUrl + 'api/Notification';
  notificationUrl = this.envService.apiUrl + 'notificationhub';

  private readonly connection = new signalR.HubConnectionBuilder()
    .withUrl(this.notificationUrl, {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .build();

  private notificationAudio = new Audio(
    '../../../assets/audios/notification.mp3'
  );

  constructor(
    private http: HttpClient,
    private envService: EnvService,
    private userStore: UserStore,
    private snackBar: MatSnackBar
  ) {}

  initConnection() {
    this.connection.on('GetNotification', (notification: NotificationModel) => {
      this.addNotificationToList(notification);
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
            this.addNotificationsToList(res, pagination.pageNumber);
        })
      );
  }

  deleteNotification(notificationId: string) {
    return this.http.delete(this.apiUrl + '/' + notificationId).pipe(
      tap(() => {
        const notifications = this.notificationsSubject.getValue();
        const index = notifications.findIndex(
          (x) => x.notificationId === notificationId
        );

        if (index !== -1) {
          notifications.splice(index, 1);
          this.notificationsSubject.next(notifications);
        }
      })
    );
  }

  markAsRead(notificationId: string) {
    return this.http
      .patch(this.apiUrl + '/' + notificationId, { notificationId })
      .pipe(
        tap(() => {
          const notifications = this.notificationsSubject.getValue();
          const index = notifications.findIndex(
            (x) => x.notificationId === notificationId
          );

          if (index !== -1) {
            notifications[index].isRead = true;
            this.notificationsSubject.next(notifications);
          }
        })
      );
  }

  addNotificationToList(notification: NotificationModel) {
    const userAuth = this.userStore.userAuth.getValue();
    if (
      notification.eventTriggererId !== userAuth?.userId &&
      notification.notificationReceiverId === userAuth?.userId
    ) {
      this.notificationsSubject.next([
        notification,
        ...this.notificationsSubject.getValue(),
      ]);

      this.playNotificationSound();
      this.showNotificationPopup(notification);
    }
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

  addNotificationsToList(
    notifications: NotificationModel[],
    pageNumber: number
  ) {
    notifications = this.filterPersonalNotifications(notifications);
    if (pageNumber === 1) this.notificationsSubject.next(notifications);
    else
      this.notificationsSubject.next([
        ...this.notificationsSubject.getValue(),
        ...notifications,
      ]);
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
