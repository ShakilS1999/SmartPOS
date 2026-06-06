import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Invoice {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Invoice`;

  downloadInvoice(saleId: number) {
    return this.http.get(`${this.apiUrl}/${saleId}`, {
      responseType: 'blob'
    });
  }

}