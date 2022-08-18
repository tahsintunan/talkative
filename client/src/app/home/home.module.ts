import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { ChatComponent } from './ui/chat/chat.component';
import { ProfileComponent } from './ui/profile/profile.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [ChatComponent, ProfileComponent],
  imports: [
    CommonModule,
    HomeRoutingModule,
    HttpClientModule
  ],
})
export class HomeModule { }
