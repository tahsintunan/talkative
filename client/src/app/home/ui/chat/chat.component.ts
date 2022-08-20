import {
  AfterViewChecked,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { chatModel } from '../../models/chat.model';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit, OnChanges, AfterViewChecked {
  @ViewChild('chatBox') private chatBox: ElementRef = new ElementRef(null);

  @Input() selectedUser?: ProfileModel;

  @Output() onClose = new EventEmitter();

  chatData: chatModel[] = [];

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    this.chatData = [
      {
        id: '1',
        message: 'Hello',
        sender: 'John Doe',
        senderId: '1',
        createdAt: new Date().toString(),
      },
      {
        id: '2',
        message: 'Hello',
        sender: 'John Doe2',
        senderId: '2',
        createdAt: new Date().toString(),
      },
      {
        id: '3',
        message: 'Hello again',
        sender: 'John Doe',
        senderId: '3',
        createdAt: new Date().toString(),
      },
    ];
  }

  ngOnInit() {
    this.scrollToBottom();
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.chatBox.nativeElement.scrollTop =
        this.chatBox?.nativeElement.scrollHeight;
    } catch (err) {}
  }

  onSend(message: string) {
    this.chatData.push({
      id: '1',
      message: message,
      sender: 'John Doe',
      senderId: '1',
      createdAt: new Date().toString(),
    });

    this.chatData = this.chatData.slice();
  }
}
