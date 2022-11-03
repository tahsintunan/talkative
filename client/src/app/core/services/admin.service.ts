import { HttpClient, HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  authUrl = this.envService.apiUrl + 'api';

  constructor(private http: HttpClient, private envService: EnvService) {}

  getUserList(pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.authUrl + '/user', {
      params: { ...pagination },
    });
  }

  banUser(userId: string) {
    return this.http.delete(this.authUrl + '/user/ban/' + userId);
  }

  unbanUser(userId: string) {
    return this.http.put(this.authUrl + '/user/unban/' + userId, null);
  }

  searchUser(search: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(
      this.authUrl +
        '/search/user/' +
        new HttpUrlEncodingCodec().encodeValue(search),
      {
        params: { ...pagination },
      }
    );
  }
}
