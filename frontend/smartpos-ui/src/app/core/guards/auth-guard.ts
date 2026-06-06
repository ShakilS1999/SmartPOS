import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('token');

  if (token) {
    return true;
  }

  router.navigate(['']);
  return false;
};

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('token');

  if (!token) {
    router.navigate(['']);
    return false;
  }

  // JWT token decode করে role check করুন
  const payload = JSON.parse(atob(token.split('.')[1]));
  const role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

  if (role === 'Admin') {
    return true;
  }

  router.navigate(['/dashboard']);
  return false;
};