import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Purchase } from '../../services/purchase';
import { Product } from '../../services/product';
import { SupplierService } from '../../services/supplier';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-purchase',
  imports: [CommonModule, FormsModule],
  templateUrl: './purchase.html',
  styleUrl: './purchase.css'
})
export class PurchaseComponent implements OnInit {

  private purchaseService = inject(Purchase);
  private productService = inject(Product);
  private supplierService = inject(SupplierService);
  private toast = inject(ToastService);

  products = signal<any[]>([]);
  cart = signal<any[]>([]);
  suppliers = signal<any[]>([]);
  purchases = signal<any[]>([]);

  selectedProductId = 0;
  selectedSupplierId = 0;
  quantity = 1;
  costPrice = 0;

  grandTotal = computed(() =>
    this.cart().reduce((sum, item) => sum + item.quantity * item.costPrice, 0)
  );

  ngOnInit(): void {
    this.loadProducts();
    this.loadSuppliers();
    this.loadPurchases();
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (res: any) => this.products.set(res),
      error: (err) => console.log(err)
    });
  }

  loadSuppliers() {
    this.supplierService.getAll().subscribe({
      next: (res) => this.suppliers.set(res),
      error: (err) => console.log(err)
    });
  }

  loadPurchases() {
    this.purchaseService.getAllPurchases().subscribe({
      next: (res) => this.purchases.set(res),
      error: (err) => console.log(err)
    });
  }

  addToCart() {
    if (this.selectedProductId === 0) {
      this.toast.warning('Please select a product');
      return;
    }
    if (this.quantity <= 0) {
      this.toast.warning('Quantity must be greater than 0');
      return;
    }
    if (this.costPrice <= 0) {
      this.toast.warning('Cost price must be greater than 0');
      return;
    }

    const product = this.products().find(
      p => p.productId === Number(this.selectedProductId)
    );

    const existing = this.cart().find(
      c => c.productId === Number(this.selectedProductId)
    );

    if (existing) {
      this.cart.update(items =>
        items.map(item =>
          item.productId === Number(this.selectedProductId)
            ? { ...item, quantity: item.quantity + this.quantity }
            : item
        )
      );
    } else {
      this.cart.update(items => [...items, {
        productId: product.productId,
        productName: product.productName,
        quantity: this.quantity,
        costPrice: this.costPrice
      }]);
    }

    this.selectedProductId = 0;
    this.quantity = 1;
    this.costPrice = 0;
  }

  removeFromCart(productId: number) {
    this.cart.update(items =>
      items.filter(c => c.productId !== productId)
    );
  }

  completePurchase() {
    if (this.cart().length === 0) {
      this.toast.warning('Cart is empty');
      return;
    }

    const data = {
      supplierId: this.selectedSupplierId === 0 ? null : this.selectedSupplierId,
      items: this.cart().map(c => ({
        productId: c.productId,
        quantity: c.quantity,
        costPrice: c.costPrice
      }))
    };

    this.purchaseService.createPurchase(data).subscribe({
      next: () => {
        this.toast.success('Purchase Completed!');
        this.cart.set([]);
        this.selectedSupplierId = 0;
        this.loadPurchases();
        this.loadProducts();
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Purchase Failed!');
      }
    });
  }

}