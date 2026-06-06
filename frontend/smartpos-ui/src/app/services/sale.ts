import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Sale {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Sale`;

  getAllSales() {
    return this.http.get(this.apiUrl);
  }

  createSale(data: any) {
    return this.http.post(this.apiUrl, data, {
      responseType: 'text'
    });
  }

  
  
  getSaleById(id: number) {
  return this.http.get<any>(`${this.apiUrl}/${id}`);
}

}