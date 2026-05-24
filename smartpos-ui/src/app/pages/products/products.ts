import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ApiService } from '../../services/api';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class ProductsComponent implements OnInit {

  products: any[] = [];

  constructor(
    private api: ApiService,
    private cd: ChangeDetectorRef,   // 👈 add
    private router: Router
  ) { }

  ngOnInit() {
    this.loadProducts();
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }




  loadProducts() {
    this.api.getProducts().subscribe({
      next: (res: any) => {
        this.products = res;
        this.cd.detectChanges();   // 👈 FIX
      },
      error: (err) => {
        console.log(err);
        alert('Failed to load products');
      }
    });
  }
}
