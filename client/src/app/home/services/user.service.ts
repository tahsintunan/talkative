import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProfileModel } from '../models/profile.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl = "http://localhost:5000/api/User/"

  constructor(
    private http: HttpClient
  ) { }

  getUser(id: string): Observable<ProfileModel> {
    let headers = new HttpHeaders()
    headers.set("Cookie", document.cookie)
    return this.http.get<ProfileModel>(this.apiUrl + id, { headers: headers, withCredentials: true })
  }
}
