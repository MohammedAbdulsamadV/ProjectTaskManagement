import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5026/api';

  constructor(private http: HttpClient) {}

  // ============= Projects =============
  getProjects(): Observable<any> { return this.http.get(`${this.baseUrl}/projects`); }
  createProject(model: any): Observable<any> { return this.http.post(`${this.baseUrl}/projects`, { model }); }
  deleteProject(id: number): Observable<any> { return this.http.delete(`${this.baseUrl}/projects/${id}`); }
updateProject(id: number, model: any): Observable<any> { return this.http.put(`${this.baseUrl}/projects/${id}`, { model }); 
  }
  // ============= Tasks =============
  getTasks(projectId: number): Observable<any> { return this.http.get(`${this.baseUrl}/tasks/project/${projectId}`); }
  createTask(model: any): Observable<any> { return this.http.post(`${this.baseUrl}/tasks`, { model }); }
  updateTaskStatus(model: any): Observable<any> { return this.http.put(`${this.baseUrl}/tasks/status`, { model }); }
  deleteTask(id: number): Observable<any> { return this.http.delete(`${this.baseUrl}/tasks/${id}`); }
}