import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'https://localhost:5026/api/tasks';

  constructor(private http: HttpClient) { }

  getTasksByProject(projectId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/project/${projectId}`);
  }

  createTask(command: any): Observable<any> {
    return this.http.post(this.apiUrl, command);
  }

  updateTaskStatus(command: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/status`, command);
  }

  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}