import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { TopNavigationComponent } from './components/top-navigation/top-navigation.component';
import { LayoutComponent } from './components/layout/layout.component';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    TopNavigationComponent,
    LayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ApiInterceptor,
      multi: true
    }
  ],
  exports: [
    TopNavigationComponent
  ]
})
export class SharedModule { }
