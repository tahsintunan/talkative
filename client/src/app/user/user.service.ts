import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userUrl = "http://localhost:5001/api/User"

  constructor(
    private http: HttpClient
  ) { }

  getUsers() {
    return this.http.get(this.userUrl)
  }
}
