import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Customer {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Customer`;

  getAll() {
    return this.http.get<any[]>(this.apiUrl);
  }

  getById(id: number) {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(data: any) {
    return this.http.post(this.apiUrl, data, {
      responseType: 'text'
    });
  }

  update(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/${id}`, data, {
      responseType: 'text'
    });
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      responseType: 'text'
    });
  }

}