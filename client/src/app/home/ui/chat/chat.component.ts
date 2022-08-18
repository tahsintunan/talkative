import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subscription, interval, flatMap } from 'rxjs';
import { ChatService } from '../../chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  constructor(
    private chatService: ChatService
  ) { }
  heartBeatSubscription: Subscription | undefined;

  ngOnInit(): void {

    this.updateOnlineStatusWithInterval()
    this.getOnlineUsersWithInterval()

  }


  updateOnlineStatusWithInterval() {
    this.updateOnlineStatus()
    setInterval(() => {
      this.updateOnlineStatus()
    }, 30000);
  }

  private updateOnlineStatus() {
    this.chatService.updateCurrentUserOnlineStatus().subscribe({
      next: res => {
        console.log(res);

      },
      error: err => {
        console.log(err);

      }
    })
  }


  getOnlineUsersWithInterval() {
    this.getOnlineUsers()
    setInterval(() => {
      this.getOnlineUsers()
    }, 30000);
  }

  private getOnlineUsers() {
    this.chatService.getOnlineUsers().subscribe({
      next: res => {
        console.log(res);
      },
      error: err => {
        console.log(err)
      }
    })
  }

}
