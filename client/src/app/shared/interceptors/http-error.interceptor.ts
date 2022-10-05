import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar) {}

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

        console.error(error);

        return throwError(() => error.error);
      })
    );
  }
}
