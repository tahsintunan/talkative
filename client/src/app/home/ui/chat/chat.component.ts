import {
  AfterViewChecked,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { ChatService } from '../../services/chat.service';
import { Message } from '../../models/message.model';
import jwt_decode from "jwt-decode";
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute } from '@angular/router';
import { chatModel } from '../../models/chat.model';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewChecked {
  @ViewChild('chatBox') private chatBox: ElementRef = new ElementRef(null);
  @Input() selectedUser?: ProfileModel;

  @Output() onClose = new EventEmitter();

  chatData: chatModel[] = [];
  heartBeatSubscription: Subscription | undefined;
  message: string = ""
  msgInboxArray: Message[] = [];
  userId: string = "";
  receiverId: string = ""

  constructor(
    private chatService: ChatService,
    private cookieService: CookieService,
    private activatedRoute: ActivatedRoute
  ) { }
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

    this.scrollToBottom();

  }
  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.chatBox.nativeElement.scrollTop =
        this.chatBox?.nativeElement.scrollHeight;
    } catch (err) { }
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

  onSend(message: string) {
    let body: Message = {
      messageText: message,
      senderId: this.userId,
      receiverId: this.receiverId
    }



    this.chatService.broadcastMessage(body).subscribe({
      next: res => {
        //message sent
      },
      error: err => {
        console.log(err);

      }
    })
    // this.chatData.push({
    //   id: '1',
    //   message: message,
    //   sender: 'John Doe',
    //   senderId: '1',
    //   createdAt: new Date().toString(),
    // });

    // this.chatData = this.chatData.slice();
  }

  messageSentToUser(senderId: string, receiverId: string): boolean {
    return (
      this.userId === senderId || this.userId === receiverId
    ) && (
        this.receiverId === receiverId || this.receiverId === senderId
      )
  }

  addToInbox(obj: Message) {
    let newObj = new Message();
    newObj = { ...obj }
    if (this.messageSentToUser(newObj.senderId, newObj.receiverId)) {
      this.msgInboxArray.push(newObj)
      console.log(newObj.messageText);

    }

  }


}

