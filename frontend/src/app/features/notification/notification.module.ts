import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { SharedModule } from 'src/app/shared/shared.module';
import { NotificationRoutingModule } from './notification-routing.module';
import { NotificationsComponent } from './pages/notifications/notifications.component';
import { NotificationItemComponent } from './ui/notification-item/notification-item.component';
import { NotificationSnackbarComponent } from './ui/notification-snackbar/notification-snackbar.component';

@NgModule({
  declarations: [
    NotificationsComponent,
    NotificationSnackbarComponent,
    NotificationItemComponent,
  ],
  imports: [
    NotificationRoutingModule,
    CommonModule,
    SharedModule,
    InfiniteScrollModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatButtonModule,
  ],
})
export class NotificationModule {}