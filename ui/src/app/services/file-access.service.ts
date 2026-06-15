import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FileAccessService {

  private apiUrl = 'http://localhost:5088/api/FileAccess';

  constructor(private http: HttpClient) {}

  logAccess(userId: number, fileId: number) {
    return this.http.post(
      `${this.apiUrl}/log?userId=${userId}&fileId=${fileId}`,
      {}
    );
  }
}