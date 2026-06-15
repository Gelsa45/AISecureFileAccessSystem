import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {

  username = '';
  password = '';

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  login() {

    this.authService.login(
      this.username,
      this.password
    )
    .subscribe({

      next: (user) => {

        localStorage.setItem(
          'userId',
          user.id
        );

        localStorage.setItem(
          'userName',
          user.name
        );

        localStorage.setItem(
          'role',
          user.role
        );

        if (user.role === 'Admin') {
          this.router.navigate(['/dashboard']);
        }
        else {
          this.router.navigate(['/user-dashboard']);
        }
      },

      error: () => {
        alert('Invalid credentials');
      }

    });
  }
}