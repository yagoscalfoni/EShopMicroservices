import { check } from 'k6';
import { defaults } from '../shared/config.js';
import {
  registerUser,
  authenticateUser,
  createProduct,
  browseCatalog,
  getProductById,
  createBasket,
  getBasket,
  checkoutBasket,
  buildOrderPayload,
  createOrder,
  getOrdersByCustomer,
  wait,
} from '../shared/helpers.js';

export function setupCheckoutJourney() {
  const registration = registerUser({
    firstName: 'Journey',
    lastName: 'User',
    email: defaults.testUserEmail,
    password: defaults.testUserPassword,
  });
  const tokenResponse = authenticateUser({ email: registration.email, password: defaults.testUserPassword });
  const product = createProduct({ category: ['journey'] });
  return {
    userEmail: registration.email,
    userId: registration.id,
    token: tokenResponse.token,
    productId: product.id,
    productPrice: product.price,
  };
}

export default function checkoutJourney(data) {
  const [listRes] = browseCatalog({ category: 'journey' });
  check(listRes, { 'journey list products': (r) => r.status === 200 });
  wait();

  // PDP load for the featured product
  const productRes = getProductById(data.productId);
  check(productRes, { 'journey product detail': (r) => r.status === 200 });
  wait();

  const basketItems = [
    {
      productId: data.productId,
      productName: 'Journey Item',
      price: data.productPrice,
      quantity: 1,
      color: 'green',
    },
  ];
  createBasket(data.userEmail, basketItems);
  wait();

  // Mini-cart fetch after adding item
  const basket = getBasket(data.userEmail);
  check(basket, { 'basket ready': (r) => r.status === 200 });
  wait();

  const checkoutPayload = {
    userName: data.userEmail,
    customerId: data.userId,
    totalPrice: data.productPrice,
    firstName: 'Journey',
    lastName: 'User',
    emailAddress: data.userEmail,
    addressLine: '123 Journey Ave',
    country: 'Testland',
    state: 'TS',
    zipCode: '00000',
    cardName: 'Journey User',
    cardNumber: '4111111111111111',
    expiration: '12/30',
    cvv: '123',
    paymentMethod: 1,
  };
  checkoutBasket(checkoutPayload);
  wait();

  // Also exercise direct order creation to validate ordering write path
  const orderPayload = buildOrderPayload({
    userId: data.userId,
    userName: data.userEmail,
    productId: data.productId,
    productPrice: data.productPrice,
  });
  createOrder(orderPayload);
  wait();

  // Account order history should remain responsive after checkout
  const ordersForCustomer = getOrdersByCustomer(data.userId);
  check(ordersForCustomer, { 'orders for customer 200': (r) => r.status === 200 });
  wait();
}
