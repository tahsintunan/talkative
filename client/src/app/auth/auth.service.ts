import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(
    private http: HttpClient
  ) { }

  baseUrl = "http://localhost:5001/User/"

  signup(body) {
    return this.http.post(this.baseUrl, body);
  }

}
