import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad, CanActivateChild {

  constructor(
    private _cookieService: CookieService,
    private _router: Router
  ) { }
  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    let accessToken = this._cookieService.get('accessToken')
    if (accessToken == "") {
      console.log("what");

      this._router.navigate(['auth/login'])
    }
    return accessToken !== "";
  }

  canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    let accessToken = this._cookieService.get('accessToken')
    return accessToken !== "";
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let accessToken = this._cookieService.get('accessToken')
    console.log("what");

    if (accessToken === "") {
      this._router.navigate(['auth/login'])
    }
    return accessToken !== "";
  }


}
