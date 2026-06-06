import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Customer } from '../../services/customer';
import { ToastService } from '../../shared/toast.service';
import { Pagination } from '../../shared/pagination/pagination';

@Component({
  selector: 'app-customers',
  imports: [CommonModule, FormsModule, Pagination],
  templateUrl: './customers.html',
  styleUrl: './customers.css'
})
export class Customers implements OnInit {

  private customerService = inject(Customer);
  private toast = inject(ToastService);

  customers = signal<any[]>([]);
  currentPage = signal(1);
  itemsPerPage = 10;

  paginatedCustomers = computed(() => {
    const start = (this.currentPage() - 1) * this.itemsPerPage;
    return this.customers().slice(start, start + this.itemsPerPage);
  });

  totalPages = computed(() =>
    Math.ceil(this.customers().length / this.itemsPerPage)
  );

  isEditing = false;
  editingId = 0;
  customerName = '';
  phone = '';

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers() {
    this.customerService.getAll().subscribe({
      next: (res) => this.customers.set(res),
      error: (err) => console.log(err)
    });
  }

  saveCustomer() {
    if (!this.customerName.trim()) {
      this.toast.warning('Customer name is required');
      return;
    }

    const data = {
      customerId: this.editingId,
      customerName: this.customerName,
      phone: this.phone
    };

    if (this.isEditing) {
      this.customerService.update(this.editingId, data).subscribe({
        next: () => {
          this.toast.success('Customer Updated!');
          this.resetForm();
          this.loadCustomers();
        },
        error: (err) => {
          console.log(err);
          this.toast.error('Update Failed!');
        }
      });
    } else {
      this.customerService.create(data).subscribe({
        next: () => {
          this.toast.success('Customer Added!');
          this.resetForm();
          this.loadCustomers();
        },
        error: (err) => {
          console.log(err);
          this.toast.error('Add Failed!');
        }
      });
    }
  }

  editCustomer(customer: any) {
    this.isEditing = true;
    this.editingId = customer.customerId;
    this.customerName = customer.customerName;
    this.phone = customer.phone;
  }

  cancelEdit() {
    this.resetForm();
  }

  deleteCustomer(id: number) {
    if (!confirm('Delete this customer?')) return;

    this.customerService.delete(id).subscribe({
      next: () => {
        this.toast.success('Customer Deleted!');
        this.loadCustomers();
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Delete Failed!');
      }
    });
  }

  resetForm() {
    this.isEditing = false;
    this.editingId = 0;
    this.customerName = '';
    this.phone = '';
  }

}