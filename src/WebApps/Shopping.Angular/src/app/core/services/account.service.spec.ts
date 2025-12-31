import { TestBed } from '@angular/core/testing';
import { AccountService } from './account.service';

describe('AccountService', () => {
  let service: AccountService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccountService);
  });

  it('should expose a complete customer journey with mock data', (done) => {
    service.getJourney().subscribe((journey) => {
      expect(journey.profile.name).toBeTruthy();
      expect(journey.overview.benefits.length).toBeGreaterThan(0);
      expect(journey.addresses.some((a) => a.default)).toBeTrue();
      expect(journey.paymentMethods.length).toBeGreaterThan(0);
      expect(journey.supportTickets.length).toBeGreaterThan(0);
      done();
    });
  });

  it('should provide derived slices individually', (done) => {
    service.getOverview().subscribe((overview) => {
      expect(overview.lastOrderId).toContain('#');
      expect(overview.pendingActions.length).toBeGreaterThan(0);
    });

    service.getProfile().subscribe((profile) => {
      expect(profile.email).toContain('@');
    });

    service.getAddresses().subscribe((addresses) => {
      expect(addresses[0].receiver).toBeDefined();
    });

    service.getPaymentMethods().subscribe((methods) => {
      expect(methods.some((m) => m.preferred)).toBeTrue();
    });

    service.getSupportTickets().subscribe((tickets) => {
      expect(tickets.every((ticket) => ticket.id.startsWith('SUP-'))).toBeTrue();
      done();
    });
  });
});
