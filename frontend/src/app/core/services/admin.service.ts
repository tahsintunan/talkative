import { HttpClient, HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import { UserModel, UserUpdateReqModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  adminUrl = this.envService.apiUrl + 'api/Admin';

  constructor(private http: HttpClient, private envService: EnvService) {}

  getUserList(pagination: PaginationModel) {
    return this.http.get<UserModel[]>(this.adminUrl + '/user', {
      params: { ...pagination },
    });
  }

  banUser(userId: string) {
    return this.http.patch(this.adminUrl + '/user/ban/' + userId, null);
  }

  unbanUser(userId: string) {
    return this.http.patch(this.adminUrl + '/user/unban/' + userId, null);
  }

  searchUser(search: string, pagination: PaginationModel) {
    return this.http.get<UserModel[]>(
      this.adminUrl +
        '/user/search/' +
        new HttpUrlEncodingCodec().encodeValue(search),
      {
        params: { ...pagination },
      }
    );
  }

  updateUser(user: UserUpdateReqModel) {
    return this.http.put(this.adminUrl + '/user/profile', user);
  }
}
