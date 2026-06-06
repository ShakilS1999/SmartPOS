import { Routes } from '@angular/router';

import { authGuard, adminGuard } from './core/guards/auth-guard';

export const routes: Routes = [

  {
    path: '',
    loadComponent: () =>
      import('./pages/login/login').then(m => m.Login)
  },

  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/dashboard/dashboard').then(m => m.DashboardComponent)
  },

  {
    path: 'products',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/products/products').then(m => m.Products)
  },

  {
    path: 'add-product',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/add-product/add-product').then(m => m.AddProduct)
  },

  {
    path: 'edit-product/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./pages/edit-product/edit-product').then(m => m.EditProduct)
  },

  {
  path: 'sales',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/sales/sales').then(m => m.Sales)
},

{
  path: 'customers',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/customers/customers').then(m => m.Customers)
},
{
  path: 'purchase',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/purchase/purchase').then(m => m.PurchaseComponent)
},
{
  path: 'profit',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/profit/profit').then(m => m.ProfitComponent)
},

{
  path: 'users',
  canActivate: [adminGuard],
  loadComponent: () =>
    import('./pages/uesrs/uesrs').then(m => m.Users)
},
{
  path: 'change-password',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/change-password/change-password').then(m => m.ChangePassword)
},
{
  path: 'returns',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/returns/returns').then(m => m.Returns)
},

{
  path: 'suppliers',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./pages/suppliers/suppliers').then(m => m.Suppliers)
},

  {
    path: '**',
    redirectTo: ''
  }

];