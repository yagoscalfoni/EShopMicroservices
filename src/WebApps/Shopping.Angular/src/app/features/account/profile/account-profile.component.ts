import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { ProfileDetails } from '../../../core/models/account.model';

@Component({
  selector: 'app-account-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-profile.component.html',
  styleUrls: ['./account-profile.component.scss']
})
export class AccountProfileComponent {
  readonly profile$: Observable<ProfileDetails> = this.route.data.pipe(
    map((data) => data['journey'].profile)
  );

  constructor(private readonly route: ActivatedRoute) {}
}
