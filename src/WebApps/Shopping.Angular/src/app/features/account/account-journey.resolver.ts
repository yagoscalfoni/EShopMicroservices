import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { AccountJourney } from '../../core/models/account.model';
import { AccountService } from '../../core/services/account.service';

export const accountJourneyResolver: ResolveFn<AccountJourney> = () =>
  inject(AccountService).getJourney();
