export interface AccountOverview {
  nextDeliveryWindow: string;
  loyaltyLevel: string;
  benefits: string[];
  lastOrderId: string;
  lastOrderTotal: number;
  pendingActions: string[];
}

export interface ProfileDetails {
  name: string;
  email: string;
  phone: string;
  document: string;
  marketingOptIn: boolean;
  securityRecommendations: string[];
}

export interface AddressSummary {
  label: string;
  receiver: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  default: boolean;
  deliveryNotes?: string;
}

export interface PaymentMethod {
  brand: string;
  last4: string;
  expiry: string;
  preferred: boolean;
  type: 'Credit Card' | 'Debit Card' | 'Pix' | 'Boleto';
}

export interface SupportTicket {
  id: string;
  subject: string;
  status: 'Aberto' | 'Respondido' | 'Concluído';
  updatedAt: string;
}

export interface AccountJourney {
  overview: AccountOverview;
  profile: ProfileDetails;
  addresses: AddressSummary[];
  paymentMethods: PaymentMethod[];
  supportTickets: SupportTicket[];
}
