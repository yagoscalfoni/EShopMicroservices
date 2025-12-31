import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ProfileDetails } from '../../../core/models/account.model';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-account-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-profile.component.html',
  styleUrls: ['./account-profile.component.scss']
})
export class AccountProfileComponent {
  readonly profile$: Observable<ProfileDetails> = this.accountService.getProfile();

  constructor(private readonly accountService: AccountService) {}
}
