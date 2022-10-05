import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { SharedModule } from '../shared/shared.module';
import { FeedComponent } from './feature/feed/feed.component';
import { HomeComponent } from './feature/home/home.component';
import { NotificationsComponent } from './feature/notifications/notifications.component';
import { ProfileComponent } from './feature/profile/profile.component';
import { HomeRoutingModule } from './home-routing.module';
import { FeedItemComponent } from './ui/feed-item/feed-item.component';
import { NavLinkItemComponent } from './ui/nav-link-item/nav-link-item.component';
import { NavbarComponent } from './ui/navbar/navbar.component';
import { PostMakerDialogComponent } from './ui/post-maker-dialog/post-maker-dialog.component';
import { ProfileDetailsComponent } from './ui/profile-details/profile-details.component';

@NgModule({
  declarations: [
    HomeComponent,
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
    MatDialogModule,
    MatMenuModule,
  ],
})
export class HomeModule {}
