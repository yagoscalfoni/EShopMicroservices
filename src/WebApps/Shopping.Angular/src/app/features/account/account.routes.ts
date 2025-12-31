import { Routes } from '@angular/router';
import {
  accountAddressesResolver,
  accountOverviewResolver,
  accountPaymentMethodsResolver,
  accountProfileResolver,
  accountSupportTicketsResolver
} from './account.resolvers';

export const accountRoutes: Routes = [
  {
    path: '',
    redirectTo: 'resumo',
    pathMatch: 'full'
  },
  {
    path: 'resumo',
    resolve: { overview: accountOverviewResolver },
    loadComponent: () => import('./overview/account-overview.component').then((m) => m.AccountOverviewComponent)
  },
  {
    path: 'perfil',
    resolve: { profile: accountProfileResolver },
    loadComponent: () => import('./profile/account-profile.component').then((m) => m.AccountProfileComponent)
  },
  {
    path: 'enderecos',
    resolve: { addresses: accountAddressesResolver },
    loadComponent: () => import('./addresses/account-addresses.component').then((m) => m.AccountAddressesComponent)
  },
  {
    path: 'pagamentos',
    resolve: { paymentMethods: accountPaymentMethodsResolver },
    loadComponent: () => import('./payments/account-payments.component').then((m) => m.AccountPaymentsComponent)
  },
  {
    path: 'suporte',
    resolve: { supportTickets: accountSupportTicketsResolver },
    loadComponent: () => import('./support/account-support.component').then((m) => m.AccountSupportComponent)
  }
];
