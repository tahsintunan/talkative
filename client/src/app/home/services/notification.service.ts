import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, tap } from 'rxjs';
import { NotificationSnackbarComponent } from 'src/app/home/ui/notification/notification-snackbar/notification-snackbar.component';
import { EnvService } from '../../env.service';
import { NotificationModel } from '../models/notification.model';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';
import { UserService } from './user.service';

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

  private userAuth?: UserModel;

  private connection = new signalR.HubConnectionBuilder()
    .withUrl(this.notificationUrl, {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .build();

  constructor(
    private http: HttpClient,
    private envService: EnvService,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }

  createConnection() {
    this.connection.onclose(async (err) => {
      await this.start();
    });
    this.connection.on('GetNotification', (notification: NotificationModel) =>
      this.addNotificationToList(notification)
    );
    this.start();
  }

  async start() {
    try {
      this.connection.start();
      console.log('connection started');
    } catch (err) {
      console.log(err);
      setTimeout(() => {
        this.start();
      }, 2500);
    }
  }

  async stop() {
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
    if (
      notification.eventTriggererId !== this.userAuth?.userId &&
      notification.notificationReceiverId === this.userAuth?.userId
    ) {
      this.notificationsSubject.next([
        notification,
        ...this.notificationsSubject.getValue(),
      ]);

      this.showNotificationPopup(notification);
    }
  }

  showNotificationPopup(notification: NotificationModel) {
    new Audio('../../../assets/audios/notification.wav').play();

    this.snackBar.openFromComponent(NotificationSnackbarComponent, {
      duration: 10000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      data: notification,
    });
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
    return notifications.filter(
      (notification) =>
        notification.eventTriggererId !== this.userAuth?.userId &&
        notification.notificationReceiverId === this.userAuth?.userId
    );
  }
}
