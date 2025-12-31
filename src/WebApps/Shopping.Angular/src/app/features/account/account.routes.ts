import { Routes } from '@angular/router';

export const accountRoutes: Routes = [
  {
    path: '',
    redirectTo: 'resumo',
    pathMatch: 'full'
  },
  {
    path: 'resumo',
    loadComponent: () => import('./overview/account-overview.component').then((m) => m.AccountOverviewComponent)
  },
  {
    path: 'perfil',
    loadComponent: () => import('./profile/account-profile.component').then((m) => m.AccountProfileComponent)
  },
  {
    path: 'enderecos',
    loadComponent: () => import('./addresses/account-addresses.component').then((m) => m.AccountAddressesComponent)
  },
  {
    path: 'pagamentos',
    loadComponent: () => import('./payments/account-payments.component').then((m) => m.AccountPaymentsComponent)
  },
  {
    path: 'suporte',
    loadComponent: () => import('./support/account-support.component').then((m) => m.AccountSupportComponent)
  }
];
