import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { DashboardModel } from '../models/dashboard.models';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  model!: DashboardModel;
  constructor(private dashboardService: DashboardService) { }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.dashboardService.load().subscribe(result => {
      this.model = result;
    });
  }

  onClick(event: Event) {
    console.log(event.target);
  }
}
