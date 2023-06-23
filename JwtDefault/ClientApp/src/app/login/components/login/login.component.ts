import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizationService } from '@shared/services/authorization.service';
import { LoginModel } from '../../models/login.models';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

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
    this.loginService.authenticate(model).subscribe(result =>
    {
      this.authService.signIn(result.token);
      this.router.navigate(['./dashboard']);
    });
  }
}
