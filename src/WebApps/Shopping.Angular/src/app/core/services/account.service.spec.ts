import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AccountService, DEFAULT_ACCOUNT_USER_ID } from './account.service';
import { AccountOverview, AddressSummary, PaymentMethod, ProfileDetails, SupportTicket } from '../models/account.model';
import { environment } from '../../../environments/environment';

describe('AccountService', () => {
  let service: AccountService;
  let httpMock: HttpTestingController;

  const overviewResponse: AccountOverview = {
    nextDeliveryWindow: 'Entrega prevista entre 10h - 14h no dia 25/06',
    loyaltyLevel: 'Cliente Gold',
    benefits: ['Frete grátis acima de R$ 99', 'Troca rápida em até 30 dias', 'Atendimento prioritário'],
    lastOrderId: '#548712',
    lastOrderTotal: 389.9,
    pendingActions: ['Adicionar endereço de trabalho', 'Salvar forma de pagamento preferida', 'Ativar notificação de ofertas']
  };

  const profileResponse: ProfileDetails = {
    name: 'Mariana Silva',
    email: 'mariana.silva@email.com',
    phone: '+55 (11) 98888-1111',
    document: '123.456.789-10',
    marketingOptIn: true,
    securityRecommendations: ['Ative a confirmação em dois fatores', 'Atualize sua senha a cada 90 dias']
  };

  const addressResponse: AddressSummary[] = [
    {
      label: 'Casa',
      receiver: 'Mariana Silva',
      street: 'Rua das Flores, 123 - Apto 45',
      city: 'São Paulo',
      state: 'SP',
      zipCode: '01310-930',
      default: true,
      deliveryNotes: 'Interfone na portaria e suba até o 4º andar'
    }
  ];

  const paymentMethodsResponse: PaymentMethod[] = [
    { brand: 'Visa', last4: '4829', expiry: '08/27', preferred: true, type: 'Credit Card' }
  ];

  const ticketsResponse: SupportTicket[] = [
    { id: 'SUP-1023', subject: 'Status do pedido #548712', status: 'Respondido', updatedAt: '2024-06-20' }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule] });
    service = TestBed.inject(AccountService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch each slice independently with dedicated endpoints', (done) => {
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

    httpMock.expectOne(`${environment.apiBaseUrl}/user-service/account/overview/${DEFAULT_ACCOUNT_USER_ID}`)
      .flush({ overview: overviewResponse });
    httpMock.expectOne(`${environment.apiBaseUrl}/user-service/account/profile/${DEFAULT_ACCOUNT_USER_ID}`)
      .flush({ profile: profileResponse });
    httpMock.expectOne(`${environment.apiBaseUrl}/user-service/account/addresses/${DEFAULT_ACCOUNT_USER_ID}`)
      .flush({ addresses: addressResponse });
    httpMock.expectOne(`${environment.apiBaseUrl}/user-service/account/payments/${DEFAULT_ACCOUNT_USER_ID}`)
      .flush({ paymentMethods: paymentMethodsResponse });
    httpMock.expectOne(`${environment.apiBaseUrl}/user-service/account/support/${DEFAULT_ACCOUNT_USER_ID}`)
      .flush({ tickets: ticketsResponse });
  });

  it('should cache each slice independently to avoid repeated network calls', () => {
    service.getOverview().subscribe();
    service.getOverview().subscribe();

    const overviewRequest = httpMock.expectOne(
      `${environment.apiBaseUrl}/user-service/account/overview/${DEFAULT_ACCOUNT_USER_ID}`
    );
    overviewRequest.flush({ overview: overviewResponse });

    httpMock.verify();
  });
});
