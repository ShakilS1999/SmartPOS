import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private authService = inject(Auth);
  private router = inject(Router);

  username = '';
  password = '';
  submitted = false;

  login() {
    this.submitted = true;

    if (!this.username || !this.password) {
      return;
    }

    const data = {
      username: this.username,
      password: this.password
    };

    this.authService.login(data).subscribe({
      next: (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.log(err);
        alert('Login Failed! Check username and password.');
      }
    });
  }
}