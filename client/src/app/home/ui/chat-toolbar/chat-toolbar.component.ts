import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Homepage } from '../../feature/home/home.page';
import { ProfileModel } from '../../Models/profile.model';
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
  ) { }

  ngOnInit(): void {

  }

  closeChat() {
    this.activatedChat.updateActiveChat("");
    this.router.navigate(['/home'])
  }
}
