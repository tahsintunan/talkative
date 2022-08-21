import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProfileModel } from '../../models/profile.model';
import { ActiveChatService } from '../../services/active-chat.service';

@Component({
  selector: 'app-chat-toolbar',
  templateUrl: './chat-toolbar.component.html',
  styleUrls: ['./chat-toolbar.component.css'],
})
export class ChatToolbarComponent implements OnInit {
  @Input() selectedUser?: ProfileModel;

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
