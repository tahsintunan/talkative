import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { SharedModule } from '../shared/shared.module';
import { FeedComponent } from './feature/feed/feed.component';
import { HomeComponent } from './feature/home/home.component';
import { NotificationsComponent } from './feature/notifications/notifications.component';
import { ProfileComponent } from './feature/profile/profile.component';
import { SearchResultComponent } from './feature/search-result/search-result.component';
import { TweetDetailsComponent } from './feature/tweet-details/tweet-details.component';
import { HomeRoutingModule } from './home-routing.module';
import { CommentInputComponent } from './ui/comment/comment-input/comment-input.component';
import { CommentItemComponent } from './ui/comment/comment-item/comment-item.component';
import { NavbarComponent } from './ui/nav/navbar/navbar.component';
import { NotificationItemComponent } from './ui/notification/notification-item/notification-item.component';
import { NotificationSnackbarComponent } from './ui/notification/notification-snackbar/notification-snackbar.component';
import { PasswordUpdateDialogComponent } from './ui/profile/password-update-dialog/password-update-dialog.component';
import { ProfileDetailsComponent } from './ui/profile/profile-details/profile-details.component';
import { ProfilePeopleItemComponent } from './ui/profile/profile-people-item/profile-people-item.component';
import { ProfilePeoplesComponent } from './ui/profile/profile-peoples/profile-peoples.component';
import { ProfileUpdateDialogComponent } from './ui/profile/profile-update-dialog/profile-update-dialog.component';
import { PostMakerDialogComponent } from './ui/tweet/post-maker-dialog/post-maker-dialog.component';
import { RetweetContentComponent } from './ui/tweet/retweet-content/retweet-content.component';
import { TweetItemComponent } from './ui/tweet/tweet-item/tweet-item.component';
import { UserListItemComponent } from './ui/user-list-item/user-list-item.component';

@NgModule({
  declarations: [
    HomeComponent,
    NavbarComponent,
    FeedComponent,
    ProfileComponent,
    NotificationsComponent,
    PostMakerDialogComponent,
    TweetItemComponent,
    ProfileDetailsComponent,
    ProfileUpdateDialogComponent,
    RetweetContentComponent,
    TweetDetailsComponent,
    CommentItemComponent,
    CommentInputComponent,
    ProfilePeopleItemComponent,
    ProfilePeoplesComponent,
    SearchResultComponent,
    PasswordUpdateDialogComponent,
    UserListItemComponent,
    NotificationSnackbarComponent,
    NotificationItemComponent,
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    FormsModule,
    InfiniteScrollModule,
    MatInputModule,
    MatIconModule,
    MatFormFieldModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatToolbarModule,
    MatIconModule,
    MatRippleModule,
    MatDividerModule,
    MatButtonModule,
    MatBadgeModule,
    MatSidenavModule,
    MatDialogModule,
    MatMenuModule,
    MatTabsModule,
  ],
})
export class HomeModule {}
