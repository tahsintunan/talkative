import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EnvService {
  public apiUrl = 'http://localhost:5000/';
  public heartbeatApiUrl = 'kernel-panic.learnathon.net/api2/Heartbeat/'
  public getOnlineUserApiUrl = 'kernel-panic.learnathon.net/api4/OnlineUsers/'

  constructor() { }
}
