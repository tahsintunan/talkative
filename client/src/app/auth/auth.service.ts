import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(
    private http: HttpClient
  ) { }

  authUrl = "http://localhost:5001/api/Auth/"

  signup(body) {
    return this.http.post(this.authUrl + "signup", body);
  }

}
