import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, map, Observable, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  refreshMode: boolean = false;
  constructor(private router: Router, private authService: AuthorizationService, private jwtService: JwtHelperService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.authService.getToken();
    if (token) {
      if (this.jwtService.isTokenExpired(token) && !this.refreshMode) {
        this.refreshMode = true;
        return this.handleRefreshToken(request, next);
      } else {
        return next.handle(this.setAuthHeader(request, token));
      }
    } else {
      return next.handle(request).pipe(map(res =>
      {
        return res;
      }), catchError((error: HttpErrorResponse) => {
        if (error.status == 401) {
          this.authService.signOut();
          this.router.navigate(['./login']);
        }
        return throwError(() => error);
      }));
    }
  }

  setAuthHeader(request: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
    return request.clone({
      headers: request.headers.set("Authorization", `Bearer ${token}`)
    });
  }

  handleRefreshToken(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return this.authService.refreshToken().pipe(switchMap(newToken => {
      this.refreshMode = false;
      this.authService.signIn(newToken.token);
      return next.handle(this.setAuthHeader(request, newToken.token));
    }), catchError((error: HttpErrorResponse) => {
      this.refreshMode = false;
      if (error.status == 401) {
        this.authService.signOut();
        this.router.navigate(['./login']);
      }
      return throwError(() => error);
    }));
  }
}
