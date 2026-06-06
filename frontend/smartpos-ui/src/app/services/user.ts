import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class User {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/User`;

  getAll() {
    return this.http.get<any[]>(this.apiUrl);
  }

  create(data: any) {
    return this.http.post(this.apiUrl, data, {
      responseType: 'text'
    });
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      responseType: 'text'
    });
  }

}