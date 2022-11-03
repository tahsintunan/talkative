import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserStore } from '../store/user.store';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(
    private snackBar: MatSnackBar,
    private cookieService: CookieService,
    private userStore: UserStore
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        let errorMessage = error.error?.message
          ? error.error.message
          : error.message;

        if (error.status === 0 && error.error instanceof ProgressEvent) {
          errorMessage = 'Network error';
        }

        this.snackBar.open('Error: ' + errorMessage, 'Close', {
          duration: 4000,
          panelClass: ['error-snackbar'],
        });

        if (error.status === 401) {
          this.cookieService.delete('authorization');
          this.userStore.clearUserAuth();
        }

        console.error(error);

        return throwError(() => error.error);
      })
    );
  }
}
