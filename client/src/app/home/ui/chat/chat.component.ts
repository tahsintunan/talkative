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
    console.log(this.userId, this.receiverId);

    this.activatedRoute.params.subscribe({
      next: (res: any) => {
        this.receiverId = res.userId
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
    newObj.senderId = obj.senderId;
    newObj.receiverId = obj.receiverId
    newObj.messageText = obj.messageText;
    this.msgInboxArray.push(newObj)
    console.log(this.msgInboxArray);

  }


  checkIfMessageSentToUser(senderId: string, receiverId: string): boolean {
    console.log(senderId, receiverId);
    console.log(this.userId, this.receiverId);


    return (
      this.userId === senderId || this.userId === receiverId
    ) && (
        this.receiverId === receiverId || this.receiverId === senderId
      )
  }

}

