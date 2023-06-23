import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizationService } from '@shared/services/authorization.service';
import { catchError, of, throwError } from 'rxjs';
import { LoginModel } from '../../models/login.models';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errorMessage: string = '';

  loginForm = this.fb.group({
    userName: ['', Validators.required],
    userPassword: ['', Validators.required]
  });
  constructor(private fb: FormBuilder
    , private loginService: LoginService
    , private authService: AuthorizationService
    , private router: Router) { }

  ngOnInit(): void {
    this.authService.signOut();
  }

  onLogin(model: LoginModel) {
    this.loginService.authenticate(model).subscribe({
      next: response => {
        this.authService.signIn(response);
        this.router.navigate(['./dashboard']);
      },
      error: error => {
        if (error.status == 400) {
          this.errorMessage = error.error;
          return of();
        }
        return throwError(() => error);
      }
    });
  }
}
