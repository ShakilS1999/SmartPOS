import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
})
export class DashboardComponent implements OnInit {

  data: any;

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {
    this.api.getDashboard().subscribe({
      next: (res: any) => {
        this.data = res;
      },
      error: (err) => {
        console.log(err);
        alert('Dashboard load failed');
      }
    });
  }
}
