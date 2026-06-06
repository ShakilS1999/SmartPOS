import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SupplierService } from '../../services/supplier';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-suppliers',
  imports: [CommonModule, FormsModule],
  templateUrl: './suppliers.html',
  styleUrl: './suppliers.css'
})
export class Suppliers implements OnInit {

  private supplierService = inject(SupplierService);
  private toast = inject(ToastService);

  suppliers = signal<any[]>([]);
  isEditing = false;
  editingId = 0;
  supplierName = '';
  phone = '';
  email = '';
  address = '';

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers() {
    this.supplierService.getAll().subscribe({
      next: (res) => this.suppliers.set(res),
      error: (err) => console.log(err)
    });
  }

  saveSupplier() {
    if (!this.supplierName.trim()) {
      this.toast.warning('Supplier name is required');
      return;
    }

    const data = {
      supplierId: this.editingId,
      supplierName: this.supplierName,
      phone: this.phone,
      email: this.email,
      address: this.address
    };

    if (this.isEditing) {
      this.supplierService.update(this.editingId, data).subscribe({
        next: () => {
          this.toast.success('Supplier Updated!');
          this.resetForm();
          this.loadSuppliers();
        },
        error: (err) => {
          console.log(err);
          this.toast.error('Update Failed!');
        }
      });
    } else {
      this.supplierService.create(data).subscribe({
        next: () => {
          this.toast.success('Supplier Added!');
          this.resetForm();
          this.loadSuppliers();
        },
        error: (err) => {
          console.log(err);
          this.toast.error('Add Failed!');
        }
      });
    }
  }

  editSupplier(supplier: any) {
    this.isEditing = true;
    this.editingId = supplier.supplierId;
    this.supplierName = supplier.supplierName;
    this.phone = supplier.phone;
    this.email = supplier.email;
    this.address = supplier.address;
  }

  cancelEdit() {
    this.resetForm();
  }

  deleteSupplier(id: number) {
    if (!confirm('Delete this supplier?')) return;

    this.supplierService.delete(id).subscribe({
      next: () => {
        this.toast.success('Supplier Deleted!');
        this.loadSuppliers();
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
    this.supplierName = '';
    this.phone = '';
    this.email = '';
    this.address = '';
  }

}