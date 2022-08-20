import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ChatService } from '../../data-access/chat.service';
import { Message } from '../../Models/message.model';
import jwt_decode from "jwt-decode";
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  constructor(
    private chatService: ChatService,
    private cookieService: CookieService,
    private activatedRoute: ActivatedRoute
  ) { }
  heartBeatSubscription: Subscription | undefined;
  message: string = ""
  msgInboxArray: Message[] = [];
  userId: string = "";
  receiverId: string = ""

  ngOnInit(): void {
    this.chatService.retrieveMappedObject().subscribe((receivedObj: Message) => { this.addToInbox(receivedObj); });

    let user: any = jwt_decode(this.cookieService.get("authorization"))
    this.userId = user.user_id;

    this.activatedRoute.params.subscribe({
      next: (res: any) => {
        this.receiverId = res.userId;
        this.resetChatHistory();
      }
    })

  }


  resetChatHistory() {
    this.msgInboxArray = [];
    let chatRequestDto = {
      senderId: this.userId,
      receiverId: this.receiverId
    }
    this.chatService.getMessages(chatRequestDto).subscribe({
      next: res => {
        this.msgInboxArray = [...res]
      },
      error: err => {
        console.log(err);

      }
    })
  }


  send(): void {
    if (this.message.trim() !== "") {

      let body: Message = {
        messageText: this.message,
        senderId: this.userId,
        receiverId: this.receiverId
      }

      this.chatService.broadcastMessage(body).subscribe({
        next: res => {
          this.message = ""
        },
        error: err => {
          console.log(err);

        }
      })
    }
  }



  addToInbox(obj: Message) {
    let newObj = new Message();
    newObj = { ...obj }
    if (this.messageSentToUser(newObj.senderId, newObj.receiverId)) {
      // this.msgInboxArray.push(newObj)
      console.log(newObj.messageText);

    }
    // console.log(this.msgInboxArray);

  }


  messageSentToUser(senderId: string, receiverId: string): boolean {
    return (
      this.userId === senderId || this.userId === receiverId
    ) && (
        this.receiverId === receiverId || this.receiverId === senderId
      )
  }

}

