import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  baseUrl = 'http://localhost:5088/api/fileaccess';

  constructor(private http: HttpClient) {}

  getAlerts() {
    return this.http.get(`${this.baseUrl}/alerts`);
  }
  logAccess(userId: number, fileId: number) {
  return this.http.post(
    `http://localhost:5088/api/fileaccess/log?userId=${userId}&fileId=${fileId}`,
    {}
  );
}
}