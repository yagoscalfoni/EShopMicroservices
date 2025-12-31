import { Routes } from '@angular/router';
import { accountJourneyResolver } from './account-journey.resolver';

export const accountRoutes: Routes = [
  {
    path: '',
    redirectTo: 'resumo',
    pathMatch: 'full'
  },
  {
    path: 'resumo',
    resolve: { journey: accountJourneyResolver },
    loadComponent: () => import('./overview/account-overview.component').then((m) => m.AccountOverviewComponent)
  },
  {
    path: 'perfil',
    resolve: { journey: accountJourneyResolver },
    loadComponent: () => import('./profile/account-profile.component').then((m) => m.AccountProfileComponent)
  },
  {
    path: 'enderecos',
    resolve: { journey: accountJourneyResolver },
    loadComponent: () => import('./addresses/account-addresses.component').then((m) => m.AccountAddressesComponent)
  },
  {
    path: 'pagamentos',
    resolve: { journey: accountJourneyResolver },
    loadComponent: () => import('./payments/account-payments.component').then((m) => m.AccountPaymentsComponent)
  },
  {
    path: 'suporte',
    resolve: { journey: accountJourneyResolver },
    loadComponent: () => import('./support/account-support.component').then((m) => m.AccountSupportComponent)
  }
];
