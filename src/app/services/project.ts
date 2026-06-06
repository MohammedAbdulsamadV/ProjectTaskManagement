import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = 'https://localhost:5026/api/projects';

  constructor(private http: HttpClient) { }

  getAllProjects(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  getProjectById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  createProject(command: any): Observable<any> {
    return this.http.post(this.apiUrl, command);
  }

  updateProject(id: number, command: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, command);
  }

  deleteProject(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}