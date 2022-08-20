import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActiveChatService {

  constructor() { }
  activatedChat = new Subject<string>;

  updateActiveChat(id: string) {
    this.activatedChat.next(id);
  }

  getActivatedChat() {
    return this.activatedChat.asObservable();
  }
}
