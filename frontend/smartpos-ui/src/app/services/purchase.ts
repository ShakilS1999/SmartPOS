import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Purchase {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Purchase`;

  createPurchase(data: any) {
    return this.http.post(this.apiUrl, data, {
      responseType: 'text'
    });
  }

  getAllPurchases() {
    return this.http.get<any[]>(this.apiUrl);
  }

}