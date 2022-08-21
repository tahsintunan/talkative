import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { map, Observable } from 'rxjs';
import { ProfileModel } from '../models/profile.model';

@Injectable({
  providedIn: 'root'
})
export class UserListService {
  heartbeatApiDeployUrl = 'http://kernel-panic.learnathon.net/api2/Heartbeat/';
  heartbeatApiUrl = "http://localhost:5001/Heartbeat/"
  onlineUserApiUrl = 'http://localhost:5003/OnlineUsers';
  onlineUserDeployApiUrl = 'http://kernel-panic.learnathon.net/api4/OnlineUsers/';

  constructor(
    private http: HttpClient,
    private cookie: CookieService
  ) { }

  updateCurrentUserOnlineStatus() {
    let headers = new HttpHeaders()
    headers.set("Cookie", document.cookie)

    return this.http.get(this.heartbeatApiUrl, { headers: headers, withCredentials: true });
  }

  getOnlineUsers(): Observable<ProfileModel[]> {
    let headers = new HttpHeaders()
    headers.set("Cookie", document.cookie)

    return this.http.get<ProfileModel[]>(this.onlineUserApiUrl, { headers: headers, withCredentials: true }).pipe(
      map(res => {
        const listOfUsers: ProfileModel[] = [];
        res.forEach((user: any) => {
          let decodedUser: any = jwtDecode(this.cookie.get("authorization"))
          if (user.id !== decodedUser.user_id) {
            listOfUsers.push({ id: user.id, username: user.username })
          }
        })
        return listOfUsers;
      }),
    )
  }


}
