import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  projects: any[] = [];
  selectedProject: any = null;
  tasks: any[] = [];

  newProject = { name: '', description: '' };
  newTask = { title: '', description: '', dueDate: '', priority: 0 };

  editingProjectId: number | null = null;
  editProjectData = { name: '', description: '' };
  isLoadingTasks = false;
  
  constructor(private api: ApiService, private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadProjects();
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  // ================= Projects Logic =================

  loadProjects() {
    this.api.getProjects().subscribe({
      next: (res: any) => {
        this.projects = res.data || res.Data;
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error(err)
    });
  }

  addProject() {
    this.api.createProject(this.newProject).subscribe({
      next: (response) => {
        this.loadProjects(); 
        this.newProject = { name: '', description: '' }; 
      },
      error: (err) => {
        console.error('❌ Error adding project:', err);
      }
    });
  }

  selectProject(project: any) {
    this.selectedProject = project;
    this.cdr.detectChanges(); 
    this.loadTasks(project.id);
  }

  startEditProject(p: any, event: Event) {
    event.stopPropagation(); 
    this.editingProjectId = p.id;
    this.editProjectData = { name: p.name, description: p.description };
  }

  saveProject(id: number, event: Event) {
    event.stopPropagation();
    this.api.updateProject(id, this.editProjectData).subscribe({
      next: () => {
        this.editingProjectId = null;
        this.loadProjects(); 
        if (this.selectedProject?.id === id) {
          this.selectedProject.name = this.editProjectData.name;
        }
      },
      error: (err) => console.error('Error updating project:', err)
    });
  }

  deleteProject(id: number, event: Event) {
    event.stopPropagation();
    if (confirm('Are you sure you want to delete this project?')) {
      this.api.deleteProject(id).subscribe({
        next: () => {
          if (this.selectedProject?.id === id) this.selectedProject = null;
          this.loadProjects();
        },
        error: (err) => console.error('Error deleting project:', err)
      });
    }
  }

  // ================= Tasks Logic =================
  loadTasks(projectId: number) {
    this.isLoadingTasks = true; 
    this.api.getTasks(projectId).subscribe({
      next: (res: any) => {
        this.tasks = res.data || res.Data;
        this.isLoadingTasks = false; 
      },
      error: (err) => {
        console.error(err);
        this.isLoadingTasks = false;
      }
    });
  }

  addTask() {
    const model = { 
      ...this.newTask, 
      projectId: this.selectedProject.id,
      priority: Number(this.newTask.priority) 
    };

    this.api.createTask(model).subscribe({
      next: () => {
        this.loadTasks(this.selectedProject.id);
        this.newTask = { title: '', description: '', dueDate: '', priority: 0 };
      },
      error: (err) => console.error('Error adding task:', err)
    });
  }

  getPriorityLabel(p: any): string {
    if (p === 0 || String(p) === '0') return 'Low';
    if (p === 1 || String(p) === '1') return 'Medium';
    if (p === 2 || String(p) === '2') return 'High';
    return typeof p === 'string' ? p : 'Low';
  }

  getStatusLabel(s: any): string {
    if (s === 0 || String(s) === '0') return 'To Do';
    if (s === 1 || String(s) === '1') return 'In Progress';
    if (s === 2 || String(s) === '2') return 'Done';
    return typeof s === 'string' ? s : 'To Do';
  }

  changeTaskStatus(taskId: number, status: number) {
    // 👈 بنغير حالة التاسك في الشاشة فوراً
    const task = this.tasks.find(t => t.id === taskId);
    if (task) {
      task.status = status; // الدالة اللي تحت هترجم الرقم لاسم صح
    }
    
    this.api.updateTaskStatus({ taskId, status }).subscribe({
      next: () => {
        // اتحدثت في السيرفر خلاص والشاشة متحدثة فعلاً
      },
      error: (err) => {
        console.error(err);
        this.loadTasks(this.selectedProject.id); // رول باك لو حصل خطأ
      }
    });
  }

  trackByTaskId(index: number, task: any): number {
    return task.id;
  }

  deleteTask(id: number) {
    if (confirm('Are you sure you want to delete this task?')) {
      // 👈 السطر ده بيطير التاسك من الشاشة في أقل من ثانية بدون ما يستنى السيرفر
      this.tasks = this.tasks.filter(t => t.id !== id);
      
      this.api.deleteTask(id).subscribe({
        next: () => {
          // مش هننادي loadTasks عشان الشاشة متعملش ريفريش ويفضل الحذف لحظي
        },
        error: (err) => {
          console.error('Error deleting task:', err);
          this.loadTasks(this.selectedProject.id); // نرجع الداتا لو السيرفر فشل يمسحها
        }
      });
    }
  }
}