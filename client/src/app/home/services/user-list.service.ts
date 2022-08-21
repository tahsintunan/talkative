import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { map, Observable } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { ProfileModel } from '../models/profile.model';

@Injectable({
  providedIn: 'root',
})
export class UserListService {
  getOnlineUserApiUrl = this.env.getOnlineUserApiUrl;

  constructor(
    private http: HttpClient, 
    private cookie: CookieService,
    private env:EnvService) {}

  getOnlineUsers(): Observable<ProfileModel[]> {
    const headers = new HttpHeaders();

    headers.set('Cookie', document.cookie);

    return this.http
      .get<ProfileModel[]>(this.getOnlineUserApiUrl, {
        headers: headers,
        withCredentials: true,
      })
      .pipe(
        map((res) => {
          const listOfUsers: ProfileModel[] = [];
          res.forEach((user: any) => {
            const decodedUser: any = jwtDecode(
              this.cookie.get('authorization')
            );

            if (user.id !== decodedUser.user_id) {
              listOfUsers.push({ id: user.id, username: user.username });
            }
          });
          return listOfUsers;
        })
      );
  }
}
