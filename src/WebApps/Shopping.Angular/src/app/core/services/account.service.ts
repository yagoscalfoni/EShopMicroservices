import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, shareReplay } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AccountJourney,
  AccountOverview,
  AddressSummary,
  PaymentMethod,
  ProfileDetails,
  SupportTicket
} from '../models/account.model';

export const DEFAULT_ACCOUNT_USER_ID = 'd1f8a1a7-b7b5-4d47-a799-df891f8bb123';

interface AccountJourneyResponse {
  journey: AccountJourney;
}

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly baseUrl = environment.apiBaseUrl;
  private readonly userId = DEFAULT_ACCOUNT_USER_ID;

  private readonly journey$ = this.http
    .get<AccountJourneyResponse>(`${this.baseUrl}/user-service/account/journey/${this.userId}`)
    .pipe(
      map((response) => response.journey),
      shareReplay({ bufferSize: 1, refCount: true })
    );

  constructor(private readonly http: HttpClient) {}

  getJourney(): Observable<AccountJourney> {
    return this.journey$;
  }

  getOverview(): Observable<AccountOverview> {
    return this.journey$.pipe(map((journey) => journey.overview));
  }

  getProfile(): Observable<ProfileDetails> {
    return this.journey$.pipe(map((journey) => journey.profile));
  }

  getAddresses(): Observable<AddressSummary[]> {
    return this.journey$.pipe(map((journey) => journey.addresses));
  }

  getPaymentMethods(): Observable<PaymentMethod[]> {
    return this.journey$.pipe(map((journey) => journey.paymentMethods));
  }

  getSupportTickets(): Observable<SupportTicket[]> {
    return this.journey$.pipe(map((journey) => journey.supportTickets));
  }
}
