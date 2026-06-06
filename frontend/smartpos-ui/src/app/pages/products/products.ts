import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Product } from '../../services/product';
import { ToastService } from '../../shared/toast.service';
import { Pagination } from '../../shared/pagination/pagination';
import { ExportService } from '../../shared/export';

@Component({
  selector: 'app-products',
  imports: [CommonModule, RouterLink, FormsModule, Pagination],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {

  private productService = inject(Product);
  private toast = inject(ToastService);
  private exportService = inject(ExportService);

  products = signal<any[]>([]);
  searchText = signal('');
  currentPage = signal(1);
  itemsPerPage = 10;

  filteredProducts = computed(() => {
    const text = this.searchText().toLowerCase();
    if (!text) return this.products();
    return this.products().filter(p =>
      p.productName.toLowerCase().includes(text) ||
      p.barcode?.toLowerCase().includes(text)
    );
  });

  paginatedProducts = computed(() => {
    const start = (this.currentPage() - 1) * this.itemsPerPage;
    return this.filteredProducts().slice(start, start + this.itemsPerPage);
  });

  totalPages = computed(() =>
    Math.ceil(this.filteredProducts().length / this.itemsPerPage)
  );

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (res: any) => this.products.set(res),
      error: (err) => {
        console.log(err);
        this.toast.error('Failed to load products');
      }
    });
  }

  onSearch(text: string) {
    this.searchText.set(text);
    this.currentPage.set(1);
  }

  deleteProduct(id: number) {
    if (!confirm('Delete this product?')) return;

    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.toast.success('Product Deleted!');
        this.loadProducts();
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Delete Failed!');
      }
    });
  }

  exportExcel() {
  this.exportService.exportProducts(this.products());
}

}