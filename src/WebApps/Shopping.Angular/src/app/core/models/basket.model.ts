export interface ShoppingCartItem {
  quantity: number;
  color: string;
  price: number;
  productId: string;
  productName: string;
}

export interface ShoppingCart {
  userName: string;
  items: ShoppingCartItem[];
  totalPrice?: number;
}

export interface GetBasketResponse {
  cart: ShoppingCart;
}

export interface StoreBasketRequest {
  cart: ShoppingCart;
}

export interface StoreBasketResponse {
  userName: string;
}

export interface DeleteBasketResponse {
  isSuccess: boolean;
}

export interface BasketCheckout {
  userName: string;
  customerId: string;
  totalPrice: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
}

export interface CheckoutBasketRequest {
  basketCheckoutDto: BasketCheckout;
}

export interface CheckoutBasketResponse {
  isSuccess: boolean;
}
