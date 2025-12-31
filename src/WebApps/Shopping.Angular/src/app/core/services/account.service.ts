import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccountOverview, AddressSummary, PaymentMethod, ProfileDetails, SupportTicket } from '../models/account.model';

export const DEFAULT_ACCOUNT_USER_ID = 'd1f8a1a7-b7b5-4d47-a799-df891f8bb123';

interface AccountOverviewResponse {
  overview: AccountOverview;
}

interface ProfileDetailsResponse {
  profile: ProfileDetails;
}

interface AddressListResponse {
  addresses: AddressSummary[];
}

interface PaymentMethodsResponse {
  paymentMethods: PaymentMethod[];
}

interface SupportTicketsResponse {
  tickets: SupportTicket[];
}

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly baseUrl = environment.apiBaseUrl;
  private readonly userId = DEFAULT_ACCOUNT_USER_ID;

  constructor(private readonly http: HttpClient) {}

  getOverview(): Observable<AccountOverview> {
    return this.http
      .get<AccountOverviewResponse>(`${this.baseUrl}/user-service/account/overview/${this.userId}`)
      .pipe(map((response) => response.overview));
  }

  getProfile(): Observable<ProfileDetails> {
    return this.http
      .get<ProfileDetailsResponse>(`${this.baseUrl}/user-service/account/profile/${this.userId}`)
      .pipe(map((response) => response.profile));
  }

  getAddresses(): Observable<AddressSummary[]> {
    return this.http
      .get<AddressListResponse>(`${this.baseUrl}/user-service/account/addresses/${this.userId}`)
      .pipe(map((response) => response.addresses));
  }

  getPaymentMethods(): Observable<PaymentMethod[]> {
    return this.http
      .get<PaymentMethodsResponse>(`${this.baseUrl}/user-service/account/payments/${this.userId}`)
      .pipe(map((response) => response.paymentMethods));
  }

  getSupportTickets(): Observable<SupportTicket[]> {
    return this.http
      .get<SupportTicketsResponse>(`${this.baseUrl}/user-service/account/support/${this.userId}`)
      .pipe(map((response) => response.tickets));
  }
}
