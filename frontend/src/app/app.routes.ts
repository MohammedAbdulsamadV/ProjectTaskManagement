import { Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth';
// لسه هنعمل الكومبوننت ده في الخطوة 4
import { DashboardComponent } from './components/dashboard/dashboard'; 

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: AuthComponent },
  { path: 'dashboard', component: DashboardComponent }
];