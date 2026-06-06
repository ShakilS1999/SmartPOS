import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Product {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Product`;

  getProducts() {
    return this.http.get(this.apiUrl);
  }

  getProductById(id: number) {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  addProduct(data: any) {
    return this.http.post(this.apiUrl, data);
  }

  updateProduct(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/${id}`, data, {
      responseType: 'text'
    });
  }

  deleteProduct(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      responseType: 'text'
    });
  }

  getLowStock() {
  return this.http.get<any[]>(`${this.apiUrl}/low-stock`);
}
}