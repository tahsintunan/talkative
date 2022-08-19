import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subscription, interval, flatMap } from 'rxjs';
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

  msgDto: Message = new Message();
  msgInboxArray: Message[] = [];

  ngOnInit(): void {
  }



  send(): void {
    if (this.msgDto) {
      console.log(this.msgDto);
      let user: any = jwt_decode(this.cookieService.get("authorization"))
      let user_id: string = user.user_id;
      let body: Message = {
        messageText: this.msgDto.messageText,
        senderId: user_id,
        receiverId: this.activatedRoute.snapshot.params['userId']
      }

      this.chatService.broadcastMessage(body);
      this.msgDto.messageText = '';
    }
  }

  inputFieldChanged(event: any) {
    this.msgDto.messageText = event.target.value
  }

  addToInbox(obj: Message) {
    let newObj = new Message();
    newObj.senderId = obj.senderId;
    newObj.receiverId = obj.receiverId
    newObj.messageText = obj.messageText;
    this.msgInboxArray.push(newObj);
    console.log(this.msgInboxArray);

  }

}
