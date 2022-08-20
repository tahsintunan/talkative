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
import { ChatModel } from '../../models/chat.model';
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

  chatData: ChatModel[] = [];

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    this.chatData = [];
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

  onSend(message: string) {}
}
