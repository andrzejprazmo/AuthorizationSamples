import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardModel } from '../components/models/dashboard.models';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(private http: HttpClient) { }

  load(): Observable<DashboardModel> {
    return this.http.get<DashboardModel>('api/dashboard/load');
  }
}
