import { Component, Input, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ActiveChatService } from '../../services/active-chat.service';
import { ChatService } from '../../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css'],
})
export class UserItemComponent implements OnInit {
  @Input() profile?: ProfileModel;

  newMessageCount = 0;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private activeChat: ActiveChatService,
    private chatService: ChatService,
    private cookie: CookieService
  ) {}

  ngOnInit(): void {
    this.chatService
      .retrieveMappedObject()
      .subscribe((receivedMessage: any) => {
        this.updateNewMessageBadge(receivedMessage);
      });
  }

  profileClicked() {
    this.newMessageCount = 0;
    this.activeChat.updateActiveChat(this.profile?.id || '');
    this.router.navigate(['./', 'chat', this.profile?.id], {
      relativeTo: this.activatedRoute,
    });
  }

  updateNewMessageBadge(message: ChatModel) {
    const splittedUrl = this.router.url.split('/');
    const chatWithId = splittedUrl[splittedUrl.length - 1];

    if (
      message.receiverId === this.getUserId() &&
      message.senderId === this.profile?.id &&
      message.senderId !== chatWithId
    ) {
      this.newMessageCount += 1;
    }
  }

  getUserId(): string {
    const decodedToken: any = jwtDecode(this.cookie.get('authorization'));
    return decodedToken.user_id;
  }
}
