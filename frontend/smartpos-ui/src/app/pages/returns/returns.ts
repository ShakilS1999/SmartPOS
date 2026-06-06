import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReturnService } from '../../services/return';
import { Sale } from '../../services/sale';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-returns',
  imports: [CommonModule, FormsModule],
  templateUrl: './returns.html',
  styleUrl: './returns.css'
})
export class Returns implements OnInit {

  private returnService = inject(ReturnService);
  private saleService = inject(Sale);
  private toast = inject(ToastService);

  returns = signal<any[]>([]);
  saleDetails: any = null;
  saleId = 0;
  reason = '';
  returnQuantities: { [key: string]: number } = {};

  ngOnInit(): void {
    this.loadReturns();
  }

  loadReturns() {
    this.returnService.getAllReturns().subscribe({
      next: (res) => this.returns.set(res),
      error: (err) => console.log(err)
    });
  }

  loadSale() {
    if (!this.saleId) {
      this.toast.warning('Please enter a Sale ID');
      return;
    }

    this.saleService.getSaleById(this.saleId).subscribe({
      next: (res: any) => {
        this.saleDetails = res;
        this.returnQuantities = {};
        res.items.forEach((item: any) => {
          this.returnQuantities[item.productName] = 0;
        });
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Sale not found!');
      }
    });
  }

  calculateRefund(): number {
    if (!this.saleDetails) return 0;

    return this.saleDetails.items.reduce((total: number, item: any) => {
      const qty = this.returnQuantities[item.productName] || 0;
      return total + (qty * item.unitPrice);
    }, 0);
  }

  processReturn() {
    if (!this.reason.trim()) {
      this.toast.warning('Please enter a reason');
      return;
    }

    const items = this.saleDetails.items
      .filter((item: any) => this.returnQuantities[item.productName] > 0)
      .map((item: any) => ({
        productId: item.productId,
        quantity: this.returnQuantities[item.productName]
      }));

    if (items.length === 0) {
      this.toast.warning('Please select at least one item to return');
      return;
    }

    const data = {
      saleId: this.saleId,
      reason: this.reason,
      items: items
    };

    this.returnService.createReturn(data).subscribe({
      next: () => {
        this.toast.success('Return Processed Successfully!');
        this.saleDetails = null;
        this.saleId = 0;
        this.reason = '';
        this.loadReturns();
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Return Failed!');
      }
    });
  }

}