import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) { }

  authProdUrl = 'http://kernel-panic.learnathon.net/api2/api/Auth/';
  authDevUrl = 'http://localhost:5001/api/Auth/'
  signup(body) {
    return this.http.post(this.authDevUrl + 'signup', body);
  }

  login(body) {
    return this.http.post(this.authDevUrl + 'login', body)
  }
}
