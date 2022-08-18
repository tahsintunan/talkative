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
    const observable = interval(60000);
    // this.heartBeatSubscription = observable.pipe(
    //   // see if flatMap can be used to remove the nested subscription
    //   flatMap(() => this.chatService.updateCurrentUserOnlineStatus())
    // )
    //   .subscribe(
    //     intervalResponse => {
    //       console.log(intervalResponse);
    //       this.chatService.updateCurrentUserOnlineStatus().subscribe((heartBeatResponse) => {
    //         console.log(heartBeatResponse);
    //       }
    //       );
    //     },
    //     error => {
    //       console.log('heartbeat error');
    //       console.log(error);
    //       if (error instanceof HttpErrorResponse) {
    //         alert(error.status + ' ' + error.statusText);
    //       }
    //     },
    //     () => {
    //       console.log('complete');
    //     }
    //   );

    this.updateOnlineStatus()
    this.getOnlineUsers()
  }


  updateOnlineStatus() {
    setInterval(() => {
      this.chatService.updateCurrentUserOnlineStatus().subscribe({
        next: res => {
          console.log(res);

        },
        error: err => {
          console.log(err);

        }
      })
    }, 10000);
  }

  getOnlineUsers() {
    setInterval(() => {
      this.chatService.getOnlineUsers().subscribe({
        next: res => {
          console.log(res);
        },
        error: err => {
          console.log(err)
        }
      })
    }, 10000);
  }

}
