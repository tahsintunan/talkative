import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { catchError, map, Observable, throwError } from 'rxjs';
import { EnvService } from 'src/app/env.service';
import { SignInReqModel, SignInResModel } from './models/signin.model';
import { SignUpReqModel, SignUpResModel } from './models/signup.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  authUrl = this.envService.apiUrl + 'api/Auth/';

  constructor(
    private http: HttpClient,
    private envService: EnvService,
    private cookieService: CookieService
  ) { }

  signup(data: SignUpReqModel): Observable<SignUpResModel> {
    return this.http.post<SignUpResModel>(this.authUrl + 'signup', data).pipe(
      map((res) => {
        console.log(res);

        return res;
      }),
      catchError(this.handleError)
    );
  }

  signin(data: SignInReqModel): Observable<SignInResModel> {

    return this.http.post<SignInResModel>(this.authUrl + 'login', data, { withCredentials: true }).pipe(
      map((res) => {
        return res;
      }),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    // Handle the HTTP error here
    console.log(error.error);
    return throwError(() => error.error.message);
  }
}
