import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AddressSummary } from '../../../core/models/account.model';

@Component({
  selector: 'app-account-addresses',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-addresses.component.html'
})
export class AccountAddressesComponent {
  readonly addresses$: Observable<AddressSummary[]> = this.route.data.pipe(
    map((data) => data['journey'].addresses)
  );

  constructor(private readonly route: ActivatedRoute) {}
}
