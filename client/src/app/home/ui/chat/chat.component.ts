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
import jwt_decode from "jwt-decode";
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute } from '@angular/router';
import { chatModel } from '../../models/chat.model';
import { ProfileModel } from '../../models/profile.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewChecked {
  @ViewChild('chatBox') private chatBox: ElementRef = new ElementRef(null);
  @Input() selectedUser?: ProfileModel;

  chatData: chatModel[] = [];
  userId: string = "";
  receiverId: string = ""

  constructor(
    private chatService: ChatService,
    private cookieService: CookieService,
    private activatedRoute: ActivatedRoute,
    private userService: UserService
  ) { }
  ngOnInit(): void {
    this.chatService.retrieveMappedObject().subscribe((receivedObj: chatModel) => { this.addToInbox(receivedObj); });

    let user: any = jwt_decode(this.cookieService.get("authorization"))
    this.userId = user.user_id;

    this.activatedRoute.params.subscribe({
      next: (res: any) => {
        this.receiverId = res.userId;
        this.getUserById()
        this.resetChatHistory();
      }
    })

    this.scrollToBottom();

  }


  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  getUserById() {
    this.userService.getUser(this.receiverId).subscribe({
      next: res => {
        this.selectedUser = { ...res }
      }
    })
  }

  scrollToBottom(): void {
    try {
      this.chatBox.nativeElement.scrollTop =
        this.chatBox?.nativeElement.scrollHeight;
    } catch (err) { }
  }

  resetChatHistory() {
    this.chatData = [];
    let chatRequestDto = {
      senderId: this.userId,
      receiverId: this.receiverId
    }
    this.chatService.getMessages(chatRequestDto).subscribe({
      next: res => {
        this.chatData = [...res]
      },
      error: err => {
        console.log(err);

      }
    })
  }

  onSend(message: string) {
    let body: chatModel = {
      messageText: message,
      senderId: this.userId,
      receiverId: this.receiverId,
      datetime: new Date()
    }



    this.chatService.broadcastMessage(body).subscribe({
      next: res => {
        //message sent
      },
      error: err => {
        console.log(err);

      }
    })
  }

  messageSentToUser(senderId: string, receiverId: string): boolean {
    return (
      this.userId === senderId || this.userId === receiverId
    )
  }

  addToInbox(obj: chatModel) {
    if (this.messageSentToUser(obj.senderId, obj.receiverId)) {
      this.chatData.push(obj)
    }

  }


}

