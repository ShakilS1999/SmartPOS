import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pos.html',
})
export class PosComponent implements OnInit {

  products: any[] = [];
  cart: any[] = [];
  grandTotal: number = 0;

  invoiceItems: any[] = [];
  showInvoice: boolean = false;
  today: string = '';

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.loadProducts();
  }

  // 📦 Load products
  loadProducts() {
    this.api.getProducts().subscribe({
      next: (res: any) => {
        this.products = res;
      },
      error: (err) => {
        console.log(err);
        alert('Failed to load products');
      }
    });
  }

  // ➕ Add to cart
  addToCart(product: any) {

    const existing = this.cart.find(
      x => x.productId === product.productId
    );

    if (existing) {
      existing.quantity++;
    }
    else {

      this.cart.push({
        productId: product.productId,
        productName: product.productName,
        quantity: 1,
        unitPrice: product.price,
        totalPrice: product.price
      });
    }

    this.updateTotal();
  }

  // 🔄 Update total
  updateTotal() {

    this.cart = this.cart.filter(
      item => item.quantity > 0
    );

    this.grandTotal = this.cart.reduce((sum, item) => {

      item.totalPrice =
        item.quantity * item.unitPrice;

      return sum + item.totalPrice;

    }, 0);
  }

  // ❌ Remove item
  removeItem(index: number) {

    this.cart.splice(index, 1);

    this.updateTotal();
  }

  // 💰 Checkout
  checkout() {

    if (this.cart.length === 0) {
      alert('Cart is empty');
      return;
    }

    const sale = {

      items: this.cart.map(item => ({
        productId: item.productId,
        quantity: item.quantity
      }))
    };

    this.api.createSale(sale).subscribe({

      next: () => {

        this.invoiceItems = [...this.cart];

        this.today =
          new Date().toLocaleString();

        this.showInvoice = true;

        this.cart = [];
        this.grandTotal = 0;

        alert('Sale Completed ✅');
      },

      error: (err) => {
        console.log(err);
        alert('Sale failed ❌');
      }
    });
  }

  // 🖨️ Print
  printInvoice() {

    const printContents =
      document.getElementById('invoice')?.innerHTML;

    const originalContents =
      document.body.innerHTML;

    if (printContents) {

      document.body.innerHTML = printContents;

      window.print();

      document.body.innerHTML = originalContents;

      window.location.reload();
    }
  }
}
