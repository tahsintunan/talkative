import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = this.env.apiUrl + 'api/User/';

  private userAuthSubject = new BehaviorSubject<UserModel>({});
  private userProfileSubject = new BehaviorSubject<UserModel>({});

  public userAuth = this.userAuthSubject.asObservable();
  public userProfile = this.userProfileSubject.asObservable();

  constructor(
    private http: HttpClient,
    private env: EnvService,
    private cookie: CookieService
  ) {}

  public init(): void {
    this.loadUserAuthData();
  }

  public loadUserAuthData(): void {
    const token = this.cookie.get('authorization');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);

        this.userAuthSubject.next({
          id: decodedToken.user_id,
          username: decodedToken.unique_name,
          email: decodedToken.email,
        });
      } catch (error) {
        console.log(error);
      }
    }
  }

  public getUser(userId: string): Observable<UserModel> {
    return this.http.get<UserModel>(this.apiUrl + userId).pipe(
      tap((res) => {
        this.userProfileSubject.next(res);
      })
    );
  }
}
