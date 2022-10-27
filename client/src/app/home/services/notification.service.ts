import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, tap } from 'rxjs';
import { NotificationSnackbarComponent } from 'src/app/home/ui/notification-snackbar/notification-snackbar.component';
import { EnvService } from '../../env.service';
import { NotificationModel } from '../models/notification.model';
import { UserModel } from '../models/user.model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private readonly notificationsSubject = new BehaviorSubject<
    NotificationModel[]
  >([]);

  public notifications = this.notificationsSubject.asObservable();

  notificationUrl = this.envService.apiUrl + 'notificationhub';

  apiUrl = this.envService.apiUrl + 'Notification';

  private userAuth?: UserModel;

  connection = new signalR.HubConnectionBuilder()
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

    this.createConnection();
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

  getNotifications() {
    return this.http
      .get<NotificationModel[]>(this.apiUrl)
      .pipe(tap((res) => this.addNotificationsToList(res)));
  }

  addNotificationToList(notification: NotificationModel) {
    if (this.userAuth?.userId !== notification.eventTriggererId) {
      this.notificationsSubject.next([
        notification,
        ...this.notificationsSubject.getValue(),
      ]);

      this.showNotifications(notification);
    }
  }

  showNotifications(notification: NotificationModel) {
    this.snackBar.openFromComponent(NotificationSnackbarComponent, {
      duration: 40000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      data: notification,
    });
  }

  addNotificationsToList(notifications: NotificationModel[]) {
    notifications = this.filterPersonalNotifications(notifications);
    this.notificationsSubject.next(notifications);
  }

  filterPersonalNotifications(notifications: NotificationModel[]) {
    return notifications.filter((notification) => {
      return notification.eventTriggererId !== this.userAuth?.userId;
    });
  }
}
