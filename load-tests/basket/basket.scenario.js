import { check } from 'k6';
import { defaults } from '../shared/config.js';
import {
  registerUser,
  authenticateUser,
  createProduct,
  createBasket,
  getBasket,
  deleteBasket,
  checkoutBasket,
  wait,
} from '../shared/helpers.js';

export function setupBasketData() {
  const registration = registerUser({
    firstName: 'Load',
    lastName: 'Tester',
    email: defaults.testUserEmail,
    password: defaults.testUserPassword,
  });

  const auth = authenticateUser({ email: registration.email, password: defaults.testUserPassword });
  const product = createProduct({ category: ['cart-flow'] });

  return {
    userName: registration.email,
    userId: registration.id,
    token: auth.token,
    productId: product.id,
    productPrice: product.price,
  };
}

export function teardownBasketData(data) {
  if (data && data.userName) {
    deleteBasket(data.userName);
  }
}

export default function basketFlow(data) {
  const basketItems = [
    {
      productId: data.productId,
      productName: 'Cart Flow Item',
      price: data.productPrice,
      quantity: 1,
      color: 'blue',
    },
  ];

  // Cart creation/update simulates incremental cart edits
  const created = createBasket(data.userName, basketItems);
  wait();

  // Cart retrieval should remain low-latency for mini-cart displays
  const fetched = getBasket(data.userName);
  check(fetched, { 'get basket 200': (r) => r.status === 200 });
  wait();

  const checkoutPayload = {
    userName: data.userName,
    customerId: data.userId,
    totalPrice: data.productPrice,
    firstName: 'Load',
    lastName: 'Tester',
    emailAddress: data.userName,
    addressLine: '123 Cart Street',
    country: 'Testland',
    state: 'TS',
    zipCode: '00000',
    cardName: 'Load Tester',
    cardNumber: '4111111111111111',
    expiration: '12/30',
    cvv: '123',
    paymentMethod: 1,
  };

  // Checkout triggers downstream orchestration and payment capture
  checkoutBasket(checkoutPayload);
  wait();

  // Clean cart to keep data volume predictable across iterations
  deleteBasket(data.userName);
  wait();
}
