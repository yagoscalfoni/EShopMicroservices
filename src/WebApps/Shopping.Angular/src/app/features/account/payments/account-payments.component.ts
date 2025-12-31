import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { PaymentMethod } from '../../../core/models/account.model';

@Component({
  selector: 'app-account-payments',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-payments.component.html'
})
export class AccountPaymentsComponent {
  readonly methods$: Observable<PaymentMethod[]> = this.route.data.pipe(
    map((data) => data['journey'].paymentMethods)
  );

  constructor(private readonly route: ActivatedRoute) {}
}
