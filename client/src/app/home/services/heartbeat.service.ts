import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from 'src/app/env.service';

@Injectable({
  providedIn: 'root',
})
export class HeartbeatService {
  heartbeatApiUrl = this.env.heartbeatApiUrl;

  constructor(private http: HttpClient, private env: EnvService) {}

  sendHeartbeat() {
    return this.http.get(this.heartbeatApiUrl);
  }
}
