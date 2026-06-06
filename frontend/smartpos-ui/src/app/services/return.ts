import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ReturnService {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Return`;

  createReturn(data: any) {
    return this.http.post(this.apiUrl, data, {
      responseType: 'text'
    });
  }

  getAllReturns() {
    return this.http.get<any[]>(this.apiUrl);
  }

}