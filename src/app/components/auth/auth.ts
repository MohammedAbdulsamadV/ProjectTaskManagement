import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router'; // 👈 الإمبورت بتاع الراوتر
import { AuthService } from '../../services/auth'; // تأكد إن مسار السيرفيس صح عندك

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './auth.html',
  styleUrl: './auth.css'
})
export class AuthComponent {
  isLoginMode = true;

  loginModel = {
    email: '',
    password: ''
  };

  registerModel = {
    userName: '',
    email: '',
    password: ''
  };

  // 👈 ضفنا الـ Router هنا جنب الـ AuthService
  constructor(private authService: AuthService, private router: Router) {}

  toggleMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  onLogin() {
    // تغليف الداتا بالظبط كـ Command للـ MediatR
    const command = { model: this.loginModel };

    this.authService.login(command).subscribe({
      next: (response) => {
        console.log('API Response:', response);

        // الباك أند بيرجع ApiResponse<AuthResponseDto> 
        // بنشيك الأول: هل فيه response، وهل جواه data، وهل الـ data جواها token؟
        if (response && response.data && (response.data.token || response.data.Token)) {
          
          // 👈 حطينا رسالة النجاح هنا! مش هتطلع غير لو استلمنا التوكن فعلاً
          alert('Login successful! 🎉'); 

          const token = response.data.token || response.data.Token;
          localStorage.setItem('token', token);
          console.log('Token saved to LocalStorage:', token);
          
          this.router.navigate(['/dashboard']); // يحولك للداشبورد
          
        } else {
          // 👈 لو الطلب نجح كـ Network بس الباك أند مرجعش توكن (يعني البيانات غلط)
          alert('Invalid email or password.');
        }
      },
      error: (err) => {
        // لو الباك أند رمى Exception أو Status 400/401 صريحة
        console.error('Server Error:', err);
        alert('Invalid email or password.');
      }
    });
  }

  onRegister() {
    const command = { model: this.registerModel };

    this.authService.register(command).subscribe({
      next: (response) => {
        alert('Registration successful! Please login.');
        this.isLoginMode = true; // يحوله لصفحة اللوجين بعد ما يسجل
      },
      error: (err) => {
        console.error('Server Error:', err);
        alert('Registration failed.');
      }
    });
  }
}