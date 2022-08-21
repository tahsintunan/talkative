import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ActiveChatService {
  activatedChat = new Subject<string>();

  constructor() {}

  updateActiveChat(id: string) {
    this.activatedChat.next(id);
  }

  getActivatedChat() {
    return this.activatedChat.asObservable();
  }
}
