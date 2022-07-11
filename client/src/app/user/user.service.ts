import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userProdUrl = "http://kernel-panic.learnathon.net/api2/api/User/";
  private userDevUrl = "http://localhost:5001/api/User/"
  constructor(
    private http: HttpClient,
    private cookieService: CookieService
  ) { }

  getUsers() {
    return this.http.get(this.userDevUrl, {
      headers: this.getHttpAuthorizationHeader()
    })
  }

  updateUser(body) {
    return this.http.put(this.userDevUrl, body, { headers: this.getHttpAuthorizationHeader() });
  }

  deleteUser(id) {
    return this.http.delete(this.userDevUrl + id, { headers: this.getHttpAuthorizationHeader() })
  }


  private getHttpAuthorizationHeader(): HttpHeaders {
    return new HttpHeaders({
      "Authorization": `Bearer ${this.cookieService.get('accessToken')}`
    })
  }
}
