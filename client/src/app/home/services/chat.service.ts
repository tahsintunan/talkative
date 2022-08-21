import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from '../../env.service';
import * as signalR from '@microsoft/signalr';
import { ChatModel } from '../models/chat.model';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private sharedObj = new Subject<ChatModel>();

  private POST_URL = 'http://localhost:5000/api/Chat/';

  private connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5000/chathub', {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .build();

  constructor(private http: HttpClient, private envService: EnvService) {
    this.connection.onclose(async (err) => {
      await this.start();
    });
    this.connection.on('ReceiveMessage', (message: ChatModel) => {
      this.mapReceivedMessage(message);
    });
    this.start();
  }

  public async start() {
    try {
      this.connection.start();
      console.log('connection started');
    } catch (err) {
      console.log(err);
      setTimeout(() => {
        this.start();
      }, 2500);
    }
  }

  broadcastMessage(message: ChatModel) {
    const headers = new HttpHeaders();

    headers.set('Cookie', document.cookie);

    return this.http.post(this.POST_URL + 'send', message, {
      headers: headers,
      withCredentials: true,
    });
  }

  private mapReceivedMessage(message: ChatModel): void {
    const receivedMessageObject: ChatModel = {
      ...message,
    };

    this.sharedObj.next(receivedMessageObject);
  }

  public retrieveMappedObject(): Observable<ChatModel> {
    return this.sharedObj.asObservable();
  }

  public getMessages(body: any): Observable<[]> {
    return this.http.post<[]>(this.POST_URL + 'GetMessageHistory', body, {
      withCredentials: true,
    });
  }
}
