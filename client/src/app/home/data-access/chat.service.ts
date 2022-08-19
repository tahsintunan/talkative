import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from '../../env.service';
import * as signalR from '@microsoft/signalr';
import { Message } from '../Models/message.model';
@Injectable({
  providedIn: 'root',
})
export class ChatService {

  private connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/chathub", {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .build();

  private POST_URL = "http://localhost:5000/api/Chat/send"
  private receivedMessageObject: Message = new Message();
  private sharedObj = new Subject<Message>();

  constructor(
    private http: HttpClient,
    private envService: EnvService,
  ) {
    this.connection.onclose(async err => {
      await this.start()
    })
    this.connection.on("ReceiveMessage", (senderId, receiverId, messageText) => { this.mapReceivedMessage(senderId, receiverId, messageText); });
    this.start();
  }


  public async start() {
    try {
      this.connection.start();
      console.log("connection started");
    } catch (err) {
      console.log(err);
      setTimeout(() => {
        this.start()
      }, 2500);
    }
  }

  broadcastMessage(message: Message) {
    let headers = new HttpHeaders()
    headers.set("Cookie", document.cookie)
    console.log(message);

    return this.http.post(this.POST_URL, message, { headers: headers, withCredentials: true }).subscribe({
      next: res => {
        console.log(res);

      },
      error: err => {
        console.log(err);

      }
    })
  }

  private mapReceivedMessage(senderId: string, receiverId: string, messageText: string): void {
    this.receivedMessageObject.senderId = senderId;
    this.receivedMessageObject.receiverId = receiverId
    this.receivedMessageObject.messageText = messageText;
    this.sharedObj.next(this.receivedMessageObject);
  }


  public retrieveMappedObject(): Observable<Message> {
    return this.sharedObj.asObservable();
  }

}
