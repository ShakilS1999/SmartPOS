import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { User } from '../../services/user';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-users',
  imports: [CommonModule, FormsModule],
  templateUrl: './uesrs.html',
  styleUrl: './uesrs.css'
})
export class Users implements OnInit {

  private userService = inject(User);
  private toast = inject(ToastService);

  users = signal<any[]>([]);
  username = '';
  password = '';
  role = 'Cashier';

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAll().subscribe({
      next: (res) => this.users.set(res),
      error: (err) => console.log(err)
    });
  }

  createUser() {
    if (!this.username.trim()) {
      this.toast.warning('Username is required');
      return;
    }
    if (!this.password.trim()) {
      this.toast.warning('Password is required');
      return;
    }

    const data = {
      username: this.username,
      password: this.password,
      role: this.role
    };

    this.userService.create(data).subscribe({
      next: () => {
        this.toast.success('User Created!');
        this.username = '';
        this.password = '';
        this.role = 'Cashier';
        this.loadUsers();
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Create Failed!');
      }
    });
  }

  deleteUser(id: number) {
    if (!confirm('Delete this user?')) return;

    this.userService.delete(id).subscribe({
      next: () => {
        this.toast.success('User Deleted!');
        this.loadUsers();
      },
      error: (err) => {
        console.log(err);
        this.toast.error('Delete Failed!');
      }
    });
  }

}
