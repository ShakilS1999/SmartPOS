import { Component, inject } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { Navbar } from './shared/navbar/navbar';
import { Toast } from './shared/toast/toast';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs/operators';
import { signal } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, Toast, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  private router = inject(Router);
  showNavbar = signal(false);

  constructor() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.showNavbar.set(event.url !== '/');
    });
  }

}