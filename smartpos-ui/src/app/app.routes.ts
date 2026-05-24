import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login';
import { ProductsComponent } from './pages/products/products';
import { PosComponent } from './pages/pos/pos';
import { DashboardComponent } from './pages/dashboard/dashboard';

import { authGuard } from './guards/auth-guard';

export const routes: Routes = [

  {
    path: '',
    component: LoginComponent
  },

  {
    path: 'products',
    component: ProductsComponent,
    canActivate: [authGuard]
  },

  {
    path: 'pos',
    component: PosComponent,
    canActivate: [authGuard]
  },

  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  }

];
