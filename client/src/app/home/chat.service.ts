import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { EnvService } from '../env.service';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  heartbeatApiDeployUrl = 'http://kernel-panic.learnathon.net/api2/Heartbeat/';
  heartbeatApiUrl = "http://localhost:5001/Heartbeat/"
  onlineUserApiUrl = 'http://localhost:5003/OnlineUsers';
  onlineUserDeployApiUrl = 'http://kernel-panic.learnathon.net/api4/OnlineUsers/';
  constructor(
    private http: HttpClient,
    private envService: EnvService,
    private cookieService: CookieService
  ) { }

  updateCurrentUserOnlineStatus() {
    let headers = new HttpHeaders()
    headers.set("Cookie", document.cookie)

    return this.http.get(this.heartbeatApiUrl, { headers: headers, withCredentials: true });
  }

  getOnlineUsers() {
    let headers = new HttpHeaders()
    headers.set("Cookie", document.cookie)

    return this.http.get(this.onlineUserApiUrl, { headers: headers, withCredentials: true });
  }


}
