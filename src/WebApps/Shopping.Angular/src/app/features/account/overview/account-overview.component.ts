import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountOverview } from '../../../core/models/account.model';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-account-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-overview.component.html'
})
export class AccountOverviewComponent {
  readonly overview$: Observable<AccountOverview> = this.accountService.getOverview();

  constructor(private readonly accountService: AccountService) {}
}
