import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProfileModel } from '../../Models/profile.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Homepage } from '../../feature/home/home.page';
import { ActiveChatService } from '../../services/active-chat.service';
@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css'],
})
export class UserItemComponent implements OnInit {
  @Input() profile?: ProfileModel;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private activeChat: ActiveChatService
  ) { }

  ngOnInit(): void { }

  profileClicked() {
    this.activeChat.updateActiveChat(this.profile?.id || "")
    this.router.navigate(['./', 'chat', this.profile?.id], { relativeTo: this.activatedRoute })
  }
}
