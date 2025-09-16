import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) { }

  getKPIs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/dashboard/kpis`);
  }

  getChartData(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/dashboard/chart`);
  }

  getMapData(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/dashboard/map`);
  }
}
