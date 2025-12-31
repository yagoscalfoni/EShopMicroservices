import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AccountService } from './account.service';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

describe('AccountService', () => {
  let service: AccountService;
  let httpMock: HttpTestingController;

  const overviewResponse = {
    overview: {
      nextDeliveryWindow: 'Entrega prevista entre 10h - 14h no dia 25/06',
      loyaltyLevel: 'Cliente Gold',
      benefits: ['Frete grátis acima de R$ 99', 'Troca rápida em até 30 dias', 'Atendimento prioritário'],
      lastOrderId: '#548712',
      lastOrderTotal: 389.9,
      pendingActions: ['Adicionar endereço de trabalho', 'Salvar forma de pagamento preferida', 'Ativar notificação de ofertas']
    }
  };

  const profileResponse = {
    profile: {
      name: 'Mariana Silva',
      email: 'mariana.silva@email.com',
      phone: '+55 (11) 98888-1111',
      document: '123.456.789-10',
      marketingOptIn: true,
      securityRecommendations: ['Ative a confirmação em dois fatores', 'Atualize sua senha a cada 90 dias']
    }
  };

  const addressesResponse = {
    addresses: [
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
    ]
  };

  const paymentsResponse = {
    paymentMethods: [{ brand: 'Visa', last4: '4829', expiry: '08/27', preferred: true, type: 'Credit Card' }]
  };

  const supportResponse = {
    tickets: [{ id: 'SUP-1023', subject: 'Status do pedido #548712', status: 'Respondido', updatedAt: '2024-06-20' }]
  };

  const authServiceStub = {
    userId: 'user-123'
  } satisfies Partial<AuthService>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: AuthService, useValue: authServiceStub }]
    });
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

    httpMock
      .expectOne(`${environment.apiBaseUrl}/user-service/account/overview/${authServiceStub.userId}`)
      .flush(overviewResponse);

    httpMock
      .expectOne(`${environment.apiBaseUrl}/user-service/account/profile/${authServiceStub.userId}`)
      .flush(profileResponse);

    httpMock
      .expectOne(`${environment.apiBaseUrl}/user-service/account/addresses/${authServiceStub.userId}`)
      .flush(addressesResponse);

    httpMock
      .expectOne(`${environment.apiBaseUrl}/user-service/account/payments/${authServiceStub.userId}`)
      .flush(paymentsResponse);

    httpMock
      .expectOne(`${environment.apiBaseUrl}/user-service/account/support/${authServiceStub.userId}`)
      .flush(supportResponse);
  });
});
