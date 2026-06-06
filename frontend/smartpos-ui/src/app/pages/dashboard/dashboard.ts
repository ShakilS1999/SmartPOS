import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Dashboard } from '../../services/dashboard';
import { Product } from '../../services/product';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {

  private dashboardService = inject(Dashboard);
  private productService = inject(Product);

  totalSales = signal(0);
  todaySales = signal(0);
  totalOrders = signal(0);
  totalProducts = signal(0);
  thisMonthSales = signal(0);
  thisMonthOrders = signal(0);
  totalProfit = signal(0);
  todayProfit = signal(0);
  lowStockProducts = signal<any[]>([]);

  ngOnInit(): void {
    this.loadData();
    this.loadLowStock();
  }

  loadData() {
    this.dashboardService.getDashboardData().subscribe({
      next: (res: any) => {
        this.totalSales.set(res.totalSales);
        this.todaySales.set(res.todaySales);
        this.totalOrders.set(res.totalOrders);
        this.totalProducts.set(res.totalProducts);
        this.thisMonthSales.set(res.thisMonthSales);
        this.thisMonthOrders.set(res.thisMonthOrders);
        this.totalProfit.set(res.totalProfit);
        this.todayProfit.set(res.todayProfit);
      },
      error: (err) => console.log(err)
    });
  }

  loadLowStock() {
    this.productService.getLowStock().subscribe({
      next: (res: any) => this.lowStockProducts.set(res),
      error: (err) => console.log(err)
    });
  }

}