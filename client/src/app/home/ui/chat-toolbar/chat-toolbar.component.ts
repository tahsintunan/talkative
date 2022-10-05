import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from '../../models/user.model';
import { ActiveChatService } from '../../services/active-chat.service';

@Component({
  selector: 'app-chat-toolbar',
  templateUrl: './chat-toolbar.component.html',
  styleUrls: ['./chat-toolbar.component.css'],
})
export class ChatToolbarComponent implements OnInit {
  @Input() selectedUser?: UserModel;

  constructor(
    private router: Router,
    private activatedChat: ActiveChatService
  ) {}

  ngOnInit(): void {}

  closeChat() {
    this.activatedChat.updateActiveChat('');
    this.router.navigate(['/home']);
  }
}
