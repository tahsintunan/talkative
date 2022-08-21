import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { ProfileModel } from '../models/profile.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = this.env.apiUrl+'api/User/';

  constructor(private http: HttpClient,private env:EnvService) {}

  getUser(id: string): Observable<ProfileModel> {
    const headers = new HttpHeaders();

    headers.set('Cookie', document.cookie);

    return this.http.get<ProfileModel>(this.apiUrl + id, {
      headers: headers,
      withCredentials: true,
    });
  }
}
