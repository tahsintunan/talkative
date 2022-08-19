import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { ChatComponent } from './ui/chat/chat.component';
import { ProfileComponent } from './ui/profile/profile.component';
import { HttpClientModule } from '@angular/common/http';
import { Homepage } from './feature/home/home.page';
import { MatIconModule } from '@angular/material/icon';
import { NavbarComponent } from './ui/navbar/navbar.component';
import { UserItemComponent } from './ui/user-item/user-item.component';
import { UserListComponent } from './ui/user-list/user-list.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@NgModule({
  declarations: [
    ChatComponent,
    ProfileComponent,
    Homepage,
    NavbarComponent,
    UserListComponent,
    UserItemComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    HomeRoutingModule,
    MatToolbarModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule
  ],
})
export class HomeModule { }
