import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { PaymentMethod } from '../../../core/models/account.model';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-account-payments',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-payments.component.html'
})
export class AccountPaymentsComponent {
  readonly methods$: Observable<PaymentMethod[]> = this.accountService.getPaymentMethods();

  constructor(private readonly accountService: AccountService) {}
}
