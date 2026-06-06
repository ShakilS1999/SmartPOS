import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class Auth {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Auth`;

  login(data: any) {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  changePassword(data: any) {
    return this.http.post(`${this.apiUrl}/change-password`, data, {
      responseType: 'text'
    });
  }

}