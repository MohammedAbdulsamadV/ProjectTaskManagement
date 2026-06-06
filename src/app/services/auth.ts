import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5026/api/auth'; 

  constructor(private http: HttpClient) { }

  register(command: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, command);
  }

  login(command: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, command);
  }
}