import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent {
  readonly navigation = [
    { label: 'Resumo', icon: 'fa-chart-pie', path: '/account/resumo' },
    { label: 'Perfil', icon: 'fa-user', path: '/account/perfil' },
    { label: 'Endereços', icon: 'fa-location-dot', path: '/account/enderecos' },
    { label: 'Pagamentos', icon: 'fa-credit-card', path: '/account/pagamentos' },
    { label: 'Suporte', icon: 'fa-headset', path: '/account/suporte' }
  ];
}
