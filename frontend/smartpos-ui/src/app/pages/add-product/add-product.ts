import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Product } from '../../services/product';
import { ToastService } from '../../shared/toast.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-product',
  imports: [FormsModule, CommonModule],
  templateUrl: './add-product.html',
  styleUrl: './add-product.css'
})
export class AddProduct {

  private productService = inject(Product);
  private router = inject(Router);
  private toast = inject(ToastService);

  productName = '';
  barcode = '';
  price = 0;
  costPrice = 0;
  stockQuantity = 0;
  generatedBarcode = signal('');

  generateBarcode() {
    const code = 'BD' + Date.now().toString().slice(-8);
    this.barcode = code;
    this.generatedBarcode.set(code);

    setTimeout(() => {
      this.renderBarcode(code);
    }, 100);
  }

  renderBarcode(code: string) {
    const JsBarcode = (window as any).JsBarcode;

    if (JsBarcode) {
      JsBarcode('#barcode-svg', code, {
        format: 'CODE128',
        width: 2,
        height: 60,
        displayValue: true
      });
    }
  }

  printBarcode() {
    const printContent = document.getElementById('barcode-print')?.innerHTML;
    const win = window.open('', '_blank');

    win?.document.write(`
      <html>
        <body onload="window.print()">
          ${printContent}
        </body>
      </html>
    `);

    win?.document.close();
  }

  saveProduct() {
    if (!this.productName.trim()) {
      this.toast.warning('Product name is required');
      return;
    }

    if (this.price <= 0) {
      this.toast.warning('Price must be greater than 0');
      return;
    }

    if (this.costPrice < 0) {
      this.toast.warning('Cost price cannot be negative');
      return;
    }

    if (this.costPrice > this.price) {
      this.toast.warning('Cost price cannot be greater than selling price');
      return;
    }

    if (this.stockQuantity < 0) {
      this.toast.warning('Stock quantity cannot be negative');
      return;
    }

    const data = {
      productName: this.productName,
      barcode: this.barcode,
      price: this.price,
      costPrice: this.costPrice,
      stockQuantity: this.stockQuantity
    };

    this.productService.addProduct(data).subscribe({
      next: () => {
        this.toast.success('Product Added!');
        this.router.navigate(['/products']);
      },
      error: (err: any) => {
        console.log(err);
        this.toast.error('Add Product Failed!');
      }
    });
  }

}