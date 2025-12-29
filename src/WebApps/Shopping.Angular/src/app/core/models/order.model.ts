export interface OrderModel {
  id: string;
  customerId: string;
  orderName: string;
  shippingAddress: AddressModel;
  billingAddress: AddressModel;
  payment: PaymentModel;
  status: OrderStatus;
  orderItems: OrderItemModel[];
}

export interface OrderItemModel {
  orderId: string;
  productId: string;
  quantity: number;
  price: number;
}

export interface AddressModel {
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
}

export interface PaymentModel {
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
}

export enum OrderStatus {
  Draft = 1,
  Pending = 2,
  Completed = 3,
  Cancelled = 4
}

export interface PaginatedResult<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: T[];
}

export interface GetOrdersResponse {
  orders: PaginatedResult<OrderModel>;
}

export interface GetOrdersByNameResponse {
  orders: OrderModel[];
}

export interface GetOrdersByCustomerResponse {
  orders: OrderModel[];
}
