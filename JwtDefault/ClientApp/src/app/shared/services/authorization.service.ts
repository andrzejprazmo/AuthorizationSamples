import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { TokenModel } from '../models/authorization.models';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  constructor(private httpClient: HttpClient, private jwtService: JwtHelperService) { }

  get storageKey(): string {
    return 'user-token';
  }

  isAuthenticated(): boolean {
    const token = sessionStorage.getItem(this.storageKey);
    return token != null && !this.jwtService.isTokenExpired(token);
  }

  signIn(token: string) {
    sessionStorage.setItem(this.storageKey, token);
  }

  signOut() {
    sessionStorage.removeItem(this.storageKey);
  }

  refreshToken(): Observable<TokenModel> {
    return this.httpClient.get<TokenModel>('api/auth/refresh-token');
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.storageKey);
  }

  isTokenExpired(): boolean {
    return false;
  }
}
