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

@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  refreshMode: boolean = false;
  constructor(private router: Router, private authService: AuthorizationService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(map(res => res), catchError((error: HttpErrorResponse) => {
      if (error.status == 401) {
        this.authService.signOut();
        this.router.navigate(['./login']);
      }
      return throwError(() => error);
    }));
  }
}
