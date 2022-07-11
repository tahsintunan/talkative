import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { EnvService } from '../env.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userUrl = this.envService.apiUrl + "api/User/"
  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    private envService: EnvService
  ) { }

  getUsers() {
    return this.http.get<any[]>(this.userUrl, {
      headers: this.getHttpAuthorizationHeader()
    })
  }

  updateUser(body) {
    return this.http.put(this.userUrl, body, { headers: this.getHttpAuthorizationHeader() });
  }

  deleteUser(id) {
    return this.http.delete(this.userUrl + id, { headers: this.getHttpAuthorizationHeader() })
  }


  private getHttpAuthorizationHeader(): HttpHeaders {
    return new HttpHeaders({
      "Authorization": `Bearer ${this.cookieService.get('accessToken')}`
    })
  }
}
