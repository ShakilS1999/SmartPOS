import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  baseUrl = 'https://localhost:7247/api';

  constructor(private http: HttpClient) { }




  login(data: any) {
    return this.http.post(`${this.baseUrl}/Auth/login`, data);
  }



  // 🔐 Token header
  getHeaders() {
    const token = localStorage.getItem('token');

    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  // 🔹 Get Products
  getProducts() {
    return this.http.get(`${this.baseUrl}/Product`, this.getHeaders());
  }

  createSale(data: any) {
    return this.http.post(`${this.baseUrl}/Sale`, data, this.getHeaders());
  }

  getDashboard() {
    return this.http.get(
      `${this.baseUrl}/Dashboard`,
      this.getHeaders()
    );
  }

}
