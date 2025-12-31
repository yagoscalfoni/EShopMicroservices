import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { SupportTicket } from '../../../core/models/account.model';

@Component({
  selector: 'app-account-support',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-support.component.html'
})
export class AccountSupportComponent {
  readonly tickets$: Observable<SupportTicket[]> = this.route.data.pipe(
    map((data) => data['supportTickets'])
  );

  constructor(private readonly route: ActivatedRoute) {}
}
