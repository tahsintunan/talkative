import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EnvService } from '../env.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private envService: EnvService
  ) { }

  authUrl = this.envService.apiUrl + "api/Auth/"

  signup(body) {
    return this.http.post(this.authUrl + 'signup', body);
  }

  login(body) {
    return this.http.post(this.authUrl + 'login', body)
  }
}
