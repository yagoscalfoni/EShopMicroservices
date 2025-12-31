import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountOverview } from '../../../core/models/account.model';

@Component({
  selector: 'app-account-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-overview.component.html'
})
export class AccountOverviewComponent {
  readonly overview$: Observable<AccountOverview> = this.route.data.pipe(
    map((data) => data['journey'].overview)
  );

  constructor(private readonly route: ActivatedRoute) {}
}
