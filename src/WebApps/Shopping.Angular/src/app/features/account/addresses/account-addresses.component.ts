import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AddressSummary } from '../../../core/models/account.model';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-account-addresses',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-addresses.component.html'
})
export class AccountAddressesComponent {
  readonly addresses$: Observable<AddressSummary[]> = this.accountService.getAddresses();

  constructor(private readonly accountService: AccountService) {}
}
