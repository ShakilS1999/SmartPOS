import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Profit } from '../../services/profit';
import { ExportService } from '../../shared/export';

@Component({
  selector: 'app-profit',
  imports: [CommonModule],
  templateUrl: './profit.html',
  styleUrl: './profit.css'
})
export class ProfitComponent implements OnInit {

  private profitService = inject(Profit);
  private exportService = inject(ExportService);

  totalProfit = signal(0);
  todayProfit = signal(0);
  totalSales = signal(0);

  ngOnInit(): void {
    this.profitService.getReport().subscribe({
      next: (res: any) => {
        this.totalProfit.set(res.totalProfit);
        this.todayProfit.set(res.todayProfit);
        this.totalSales.set(res.totalSales);
      },
      error: (err) => console.log(err)
    });
  }

  exportExcel() {
    this.exportService.exportProfit({
      totalProfit: this.totalProfit(),
      todayProfit: this.todayProfit(),
      totalSales: this.totalSales()
    });
  }

}