import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenModel } from '@shared/models/authorization.models';
import { LoginModel } from '../models/login.models';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  authenticate(model: LoginModel): Observable<TokenModel> {
    return this.http.post<TokenModel>('api/auth/authenticate', model);
  }
}
