import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-chat-toolbar',
  templateUrl: './chat-toolbar.component.html',
  styleUrls: ['./chat-toolbar.component.css'],
})
export class ChatToolbarComponent implements OnInit {
  @Input() selectedUser?: ProfileModel;


  constructor(
    private router: Router
  ) { }

  ngOnInit(): void { }

  closeChat() {
    this.router.navigate(['/home'])
  }
}
