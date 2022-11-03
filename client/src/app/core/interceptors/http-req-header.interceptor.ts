import {
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoaderService } from '../../shared/services/loader.service';

@Injectable()
export class HttpReqHeaderInterceptor implements HttpInterceptor {
  constructor(private loaderService: LoaderService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const headers = new HttpHeaders();

    headers.set('Cookie', document.cookie);

    const req = request.clone({
      headers: headers,
      withCredentials: true,
    });

    return next.handle(req);
  }
}
