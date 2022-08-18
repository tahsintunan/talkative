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

@NgModule({
  declarations: [ChatComponent, ProfileComponent, Homepage, NavbarComponent, UserListComponent, UserItemComponent],
  imports: [CommonModule, HomeRoutingModule, MatToolbarModule, MatIconModule],
})
export class HomeModule {}
