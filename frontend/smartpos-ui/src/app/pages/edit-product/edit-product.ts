import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../services/product';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-edit-product',
  imports: [FormsModule],
  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css'
})
export class EditProduct implements OnInit {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productService = inject(Product);
  private toast = inject(ToastService);

  productId = 0;
  productName = '';
  barcode = '';
  price = 0;
  costPrice = 0;
  stockQuantity = 0;

  ngOnInit(): void {
    this.productId = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.loadProduct();
  }

  loadProduct() {
    this.productService.getProductById(this.productId).subscribe({
      next: (res: any) => {
        this.productName = res.productName;
        this.barcode = res.barcode;
        this.price = res.price;
        this.costPrice = res.costPrice ?? 0;
        this.stockQuantity = res.stockQuantity;
      },
      error: (err) => console.log(err)
    });
  }

  updateProduct() {
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
      productId: this.productId,
      productName: this.productName,
      barcode: this.barcode,
      price: this.price,
      costPrice: this.costPrice,
      stockQuantity: this.stockQuantity
    };

    this.productService.updateProduct(this.productId, data).subscribe({
      next: () => {
        this.toast.success('Product Updated!');
        this.router.navigate(['/products']);
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Update Failed!');
      }
    });
  }

}