import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { AuthenticatedUser } from '../../../core/models/auth.model';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  readonly user$: Observable<AuthenticatedUser | null> = this.authService.currentUser$;
  readonly userMenu = [
    { label: 'Minha conta', path: '/account/resumo', icon: 'fa-house-user' },
    { label: 'Perfil e dados', path: '/account/perfil', icon: 'fa-id-card' },
    { label: 'Endereços', path: '/account/enderecos', icon: 'fa-location-dot' },
    { label: 'Pagamentos', path: '/account/pagamentos', icon: 'fa-credit-card' },
    { label: 'Pedidos', path: '/orders', icon: 'fa-box' },
    { label: 'Suporte', path: '/account/suporte', icon: 'fa-headset' }
  ];

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
