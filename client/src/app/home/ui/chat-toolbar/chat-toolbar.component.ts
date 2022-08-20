import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-chat-toolbar',
  templateUrl: './chat-toolbar.component.html',
  styleUrls: ['./chat-toolbar.component.css'],
})
export class ChatToolbarComponent implements OnInit {
  @Input() selectedUser?: ProfileModel;
  @Output() onClose = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}
}
