import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  templateUrl: './chat-input.component.html',
  styleUrls: ['./chat-input.component.css'],
})
export class ChatInputComponent implements OnInit {
  @Output() onSend = new EventEmitter();

  message: string = '';

  constructor() {}

  ngOnInit(): void {}

  sendMessage() {
    this.onSend.emit(this.message.trim());
    this.message = '';
  }
}
