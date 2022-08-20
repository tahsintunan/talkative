import { Component, Input, OnInit } from '@angular/core';
import { ChatModel } from '../../models/chat.model';

@Component({
  selector: 'app-chat-item',
  templateUrl: './chat-item.component.html',
  styleUrls: ['./chat-item.component.css'],
})
export class ChatItemComponent implements OnInit {
  @Input() chatData?: ChatModel;
  @Input() isSender?: boolean = false;

  constructor() {}

  ngOnInit(): void {}
}
