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
import { AlertSnackbarComponent } from 'src/app/shared/ui/alert-snackbar/alert-snackbar.component';
import { AuthService } from '../services/auth.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(
    private snackBar: MatSnackBar,
    private authService: AuthService
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

        this.snackBar.openFromComponent(AlertSnackbarComponent, {
          data: {
            title: 'Error',
            message: errorMessage,
            type: 'danger',
          },
        });

        if (error.status === 401) {
          this.authService.signout();
        }

        console.error(error);

        return throwError(() => error.error);
      })
    );
  }
}
