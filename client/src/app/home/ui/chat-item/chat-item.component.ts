import { Component, Input, OnInit } from '@angular/core';
import { chatModel } from '../../models/chat.model';

@Component({
  selector: 'app-chat-item',
  templateUrl: './chat-item.component.html',
  styleUrls: ['./chat-item.component.css'],
})
export class ChatItemComponent implements OnInit {
  @Input() chatData?: chatModel;
  @Input() isSender?: boolean = false;

  constructor() {}

  ngOnInit(): void {}
}
