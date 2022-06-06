import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userUrl = "http://kernel-panic.learnathon.net/api2/api/User/";

  constructor(
    private http: HttpClient
  ) { }

  getUsers() {
    return this.http.get(this.userUrl)
  }

  updateUser(body) {
    return this.http.put(this.userUrl, body);
  }

  deleteUser(id) {
    return this.http.delete(this.userUrl + id)
  }
}
