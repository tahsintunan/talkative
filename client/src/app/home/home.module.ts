import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { ChatComponent } from './ui/chat/chat.component';
import { ProfileComponent } from './ui/profile/profile.component';
import { Homepage } from './feature/home/home.page';
import { NavbarComponent } from './ui/navbar/navbar.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { UserListComponent } from './ui/user-list/user-list.component';
import { UserItemComponent } from './ui/user-item/user-item.component';
import { MatRippleModule } from '@angular/material/core';
import { ChatInputComponent } from './ui/chat-input/chat-input.component';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { ChatItemComponent } from './ui/chat-item/chat-item.component';
import { ChatToolbarComponent } from './ui/chat-toolbar/chat-toolbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { MatBadgeModule } from '@angular/material/badge';

@NgModule({
  declarations: [
    ChatComponent,
    ProfileComponent,
    Homepage,
    NavbarComponent,
    UserListComponent,
    UserItemComponent,
    ChatInputComponent,
    ChatItemComponent,
    ChatToolbarComponent,
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
  ],
})
export class HomeModule {}
