import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        const errorMessage = error.error?.message
          ? error.error.message
          : error.message;

        this.snackBar.open('Error: ' + errorMessage, 'Close', {
          duration: 4000,
          panelClass: ['error-snackbar'],
        });

        console.error(error.error);

        return throwError(() => error.error);
      })
    );
  }
}
