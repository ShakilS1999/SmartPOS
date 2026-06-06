import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-receipt-modal',
  imports: [CommonModule],
  templateUrl: './receipt-modal.html',
  styleUrl: './receipt-modal.css'
})
export class ReceiptModal {

  sale = input<any>(null);
  close = output<void>();

  printReceipt() {
    window.print();
  }

}