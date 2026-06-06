import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-change-password',
  imports: [FormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {

  private authService = inject(Auth);
  private router = inject(Router);
  private toast = inject(ToastService);

  oldPassword = '';
  newPassword = '';
  confirmPassword = '';

  changePassword() {
    if (!this.oldPassword || !this.newPassword || !this.confirmPassword) {
      this.toast.warning('All fields are required');
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.toast.warning('New passwords do not match');
      return;
    }

    if (this.newPassword.length < 4) {
      this.toast.warning('Password must be at least 4 characters');
      return;
    }

    // Token থেকে username নিন
    const token = localStorage.getItem('token');
    const payload = JSON.parse(atob(token!.split('.')[1]));
    const username = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    const data = {
      username: username,
      oldPassword: this.oldPassword,
      newPassword: this.newPassword
    };

    this.authService.changePassword(data).subscribe({
      next: () => {
        this.toast.success('Password Changed Successfully!');
        localStorage.removeItem('token');
        setTimeout(() => this.router.navigate(['']), 1500);
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Password Change Failed!');
      }
    });
  }

}