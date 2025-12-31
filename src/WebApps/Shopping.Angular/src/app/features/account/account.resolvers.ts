import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { AccountOverview, AddressSummary, PaymentMethod, ProfileDetails, SupportTicket } from '../../core/models/account.model';
import { AccountService } from '../../core/services/account.service';

export const accountOverviewResolver: ResolveFn<AccountOverview> = () =>
  inject(AccountService).getOverview();

export const accountProfileResolver: ResolveFn<ProfileDetails> = () =>
  inject(AccountService).getProfile();

export const accountAddressesResolver: ResolveFn<AddressSummary[]> = () =>
  inject(AccountService).getAddresses();

export const accountPaymentMethodsResolver: ResolveFn<PaymentMethod[]> = () =>
  inject(AccountService).getPaymentMethods();

export const accountSupportTicketsResolver: ResolveFn<SupportTicket[]> = () =>
  inject(AccountService).getSupportTickets();
