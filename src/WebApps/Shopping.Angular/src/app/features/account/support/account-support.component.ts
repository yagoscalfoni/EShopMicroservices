import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { SupportTicket } from '../../../core/models/account.model';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-account-support',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-support.component.html'
})
export class AccountSupportComponent {
  readonly tickets$: Observable<SupportTicket[]> = this.accountService.getSupportTickets();

  constructor(private readonly accountService: AccountService) {}
}
