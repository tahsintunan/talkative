import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { Homepage } from './feature/home/home.page';
import { NavbarComponent } from './ui/navbar/navbar.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCardModule } from '@angular/material/card';
import { NavLinkItemComponent } from './ui/nav-link-item/nav-link-item.component';
import { FeedComponent } from './feature/feed/feed.component';
import { ProfileComponent } from './feature/profile/profile.component';
import { NotificationsComponent } from './feature/notifications/notifications.component';
import { PostMakerDialogComponent } from './ui/post-maker-dialog/post-maker-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { FeedItemComponent } from './ui/feed-item/feed-item.component';
import { ProfileDetailsComponent } from './ui/profile-details/profile-details.component';

@NgModule({
  declarations: [
    Homepage,
    NavbarComponent,
    NavLinkItemComponent,
    FeedComponent,
    ProfileComponent,
    NotificationsComponent,
    PostMakerDialogComponent,
    FeedItemComponent,
    ProfileDetailsComponent,
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatIconModule,
    MatRippleModule,
    MatDividerModule,
    MatButtonModule,
    MatBadgeModule,
    MatSidenavModule,
    MatCardModule,
    MatDialogModule,
  ],
})
export class HomeModule {}
