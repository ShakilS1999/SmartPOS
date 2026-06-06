import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sale } from '../../services/sale';
import { Product } from '../../services/product';
import { Invoice } from '../../services/invoice';
import { Customer } from '../../services/customer';
import { ToastService } from '../../shared/toast.service';
import { Pagination } from '../../shared/pagination/pagination';
import { ReceiptModal } from '../../shared/receipt-modal/receipt-modal';
import { ExportService } from '../../shared/export';

@Component({
  selector: 'app-sales',
  imports: [CommonModule, FormsModule, Pagination, ReceiptModal],
  templateUrl: './sales.html',
  styleUrl: './sales.css'
})
export class Sales implements OnInit {

  private saleService = inject(Sale);
  private productService = inject(Product);
  private invoiceService = inject(Invoice);
  private customerService = inject(Customer);
  private toast = inject(ToastService);
  private exportService = inject(ExportService);

  products = signal<any[]>([]);
  sales = signal<any[]>([]);
  customers = signal<any[]>([]);
  cart: any[] = [];

  selectedProductId = 0;
  selectedCustomerId = 0;
  quantity = 1;
  discount = 0;
  tax = 0;
  paidAmount = 0;
  barcodeInput = '';

  currentPage = signal(1);
  itemsPerPage = 10;

  showReceipt = signal(false);
  receiptData = signal<any>(null);

  subTotal = computed(() =>
    this.cart.reduce((sum, item) => sum + item.quantity * item.unitPrice, 0)
  );

  netTotal = computed(() =>
    this.subTotal() - this.discount + this.tax
  );

  dueAmount = computed(() =>
    Math.max(0, this.netTotal() - this.paidAmount)
  );

  paginatedSales = computed(() => {
    const start = (this.currentPage() - 1) * this.itemsPerPage;
    return this.sales().slice(start, start + this.itemsPerPage);
  });

  totalPages = computed(() =>
    Math.ceil(this.sales().length / this.itemsPerPage)
  );

  ngOnInit(): void {
    this.loadProducts();
    this.loadSales();
    this.loadCustomers();
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (res: any) => this.products.set(res),
      error: (err) => console.log(err)
    });
  }

  loadSales() {
    this.saleService.getAllSales().subscribe({
      next: (res: any) => this.sales.set(res),
      error: (err) => console.log(err)
    });
  }

  loadCustomers() {
    this.customerService.getAll().subscribe({
      next: (res: any) => this.customers.set(res),
      error: (err) => console.log(err)
    });
  }

  onBarcodeInput(barcode: string) {
    if (!barcode.trim()) return;

    const product = this.products().find(
      p => p.barcode === barcode.trim()
    );

    if (!product) {
      this.toast.warning('Product not found!');
      this.barcodeInput = '';
      return;
    }

    const existing = this.cart.find(
      c => c.productId === product.productId
    );

    if (existing) {
      existing.quantity += 1;
    } else {
      this.cart.push({
        productId: product.productId,
        productName: product.productName,
        quantity: 1,
        unitPrice: product.price
      });
    }

    this.toast.success(`${product.productName} added!`);
    this.barcodeInput = '';
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

    const product = this.products().find(
      p => p.productId === Number(this.selectedProductId)
    );

    const existing = this.cart.find(
      c => c.productId === Number(this.selectedProductId)
    );

    if (existing) {
      existing.quantity += this.quantity;
    } else {
      this.cart.push({
        productId: product.productId,
        productName: product.productName,
        quantity: this.quantity,
        unitPrice: product.price
      });
    }

    this.selectedProductId = 0;
    this.quantity = 1;
  }

  removeFromCart(productId: number) {
    this.cart = this.cart.filter(c => c.productId !== productId);
  }

  completeSale() {
    if (this.cart.length === 0) {
      this.toast.warning('Cart is empty');
      return;
    }

    const data = {
      customerId: this.selectedCustomerId === 0 ? null : this.selectedCustomerId,
      discount: this.discount,
      tax: this.tax,
      paidAmount: this.paidAmount,
      items: this.cart.map(c => ({
        productId: c.productId,
        quantity: c.quantity
      }))
    };

    this.saleService.createSale(data).subscribe({
      next: () => {
        this.toast.success('Sale Completed!');
        this.loadSales();

        setTimeout(() => {
          const latestSale = this.sales()[0];
          if (latestSale) {
            this.receiptData.set(latestSale);
            this.showReceipt.set(true);
          }
        }, 500);

        this.cart = [];
        this.discount = 0;
        this.tax = 0;
        this.paidAmount = 0;
        this.selectedCustomerId = 0;
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Sale Failed!');
      }
    });
  }

  viewReceipt(sale: any) {
    this.receiptData.set(sale);
    this.showReceipt.set(true);
  }

  exportExcel() {
    this.exportService.exportSales(this.sales());
  }

  downloadInvoice(saleId: number) {
    this.invoiceService.downloadInvoice(saleId).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Invoice-${saleId}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Invoice Download Failed!');
      }
    });
  }

}