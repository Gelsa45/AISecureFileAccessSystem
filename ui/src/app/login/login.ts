import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {

  username: string = '';

  constructor(private router: Router) {}

  login() {
    if (this.username === 'admin') {
      this.router.navigate(['/dashboard']);
    } else {
      alert('Only admin allowed');
    }
  }
}