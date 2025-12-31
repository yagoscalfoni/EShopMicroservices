import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {
  AccountJourney,
  AccountOverview,
  AddressSummary,
  PaymentMethod,
  ProfileDetails,
  SupportTicket
} from '../models/account.model';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly journey: AccountJourney = {
    overview: {
      nextDeliveryWindow: 'Entrega prevista entre 10h - 14h no dia 25/06',
      loyaltyLevel: 'Cliente Gold',
      benefits: ['Frete grátis acima de R$ 99', 'Troca rápida em até 30 dias', 'Atendimento prioritário'],
      lastOrderId: '#548712',
      lastOrderTotal: 389.9,
      pendingActions: ['Adicionar endereço de trabalho', 'Salvar forma de pagamento preferida', 'Ativar notificação de ofertas']
    },
    profile: {
      name: 'Mariana Silva',
      email: 'mariana.silva@email.com',
      phone: '+55 (11) 98888-1111',
      document: '123.456.789-10',
      marketingOptIn: true,
      securityRecommendations: ['Ative a confirmação em dois fatores', 'Atualize sua senha a cada 90 dias']
    },
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
      },
      {
        label: 'Trabalho',
        receiver: 'Mariana Silva',
        street: 'Av. Paulista, 500 - 12º andar',
        city: 'São Paulo',
        state: 'SP',
        zipCode: '01310-100',
        default: false,
        deliveryNotes: 'Recepção aceita entregas até 18h'
      }
    ],
    paymentMethods: [
      { brand: 'Visa', last4: '4829', expiry: '08/27', preferred: true, type: 'Credit Card' },
      { brand: 'Mastercard', last4: '1038', expiry: '03/26', preferred: false, type: 'Debit Card' },
      { brand: 'Pix', last4: 'Conta Itaú', expiry: 'Disponível', preferred: false, type: 'Pix' }
    ],
    supportTickets: [
      { id: 'SUP-1023', subject: 'Status do pedido #548712', status: 'Respondido', updatedAt: 'Há 2 horas' },
      { id: 'SUP-0974', subject: 'Troca de produto - pedido #540120', status: 'Concluído', updatedAt: '05/06/2024' },
      { id: 'SUP-0955', subject: 'Alterar endereço principal', status: 'Aberto', updatedAt: '03/06/2024' }
    ]
  };

  getJourney(): Observable<AccountJourney> {
    return of(this.journey);
  }

  getOverview(): Observable<AccountOverview> {
    return of(this.journey.overview);
  }

  getProfile(): Observable<ProfileDetails> {
    return of(this.journey.profile);
  }

  getAddresses(): Observable<AddressSummary[]> {
    return of(this.journey.addresses);
  }

  getPaymentMethods(): Observable<PaymentMethod[]> {
    return of(this.journey.paymentMethods);
  }

  getSupportTickets(): Observable<SupportTicket[]> {
    return of(this.journey.supportTickets);
  }
}
