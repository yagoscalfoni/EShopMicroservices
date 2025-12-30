import http from 'k6/http';
import { check, fail, sleep } from 'k6';
import { uuidv4, randomItem } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';
import { defaults, getBaseUrl, think } from './config.js';

export function buildHeaders(token) {
  const headers = { 'Content-Type': 'application/json' };
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }
  return { headers };
}

export function authenticateUser({ email, password }) {
  const baseUrl = getBaseUrl('user');
  const payload = JSON.stringify({ email, password });
  const res = http.post(`${baseUrl}/authenticate`, payload, buildHeaders());

  check(res, {
    'authenticate status is 200': (r) => r.status === 200,
  }) || fail(`Authentication failed: ${res.status} - ${res.body}`);

  return res.json();
}

export function registerUser({ firstName, lastName, email, password }) {
  const baseUrl = getBaseUrl('user');
  const payload = JSON.stringify({ firstName, lastName, email, password });
  const res = http.post(`${baseUrl}/register`, payload, buildHeaders());

  check(res, {
    'register status is 201': (r) => r.status === 201,
  }) || fail(`User registration failed: ${res.status} - ${res.body}`);

  return res.json();
}

export function createProduct({ namePrefix = 'LoadTest Product', category = ['general'] }) {
  const baseUrl = getBaseUrl('catalog');
  const body = JSON.stringify({
    name: `${namePrefix} ${uuidv4()}`,
    category,
    description: 'Synthetic product used for load testing',
    imageFile: 'placeholder.png',
    price: randomPrice(),
  });

  const res = http.post(`${baseUrl}/products`, body, buildHeaders());
  check(res, {
    'product created': (r) => r.status === 201,
  }) || fail(`Failed to create product: ${res.status} - ${res.body}`);

  return res.json();
}

export function updateProduct(productId, updatePayload) {
  const baseUrl = getBaseUrl('catalog');
  const res = http.put(`${baseUrl}/products`, JSON.stringify(updatePayload), buildHeaders());
  check(res, {
    'product updated': (r) => r.status === 200,
  }) || fail(`Failed to update product: ${res.status} - ${res.body}`);
  return res.json();
}

export function deleteProduct(productId) {
  const baseUrl = getBaseUrl('catalog');
  const res = http.del(`${baseUrl}/products/${productId}`, null, buildHeaders());
  check(res, {
    'product deleted': (r) => r.status === 200,
  });
}

export function getProductById(productId) {
  const baseUrl = getBaseUrl('catalog');
  return http.get(`${baseUrl}/products/${productId}`, buildHeaders());
}

export function browseCatalog({ category }) {
  const baseUrl = getBaseUrl('catalog');
  const responses = [];
  responses.push(http.get(`${baseUrl}/products`, buildHeaders()));
  responses.push(http.get(`${baseUrl}/products/category/${category}`, buildHeaders()));
  return responses;
}

export function createBasket(userName, items) {
  const baseUrl = getBaseUrl('basket');
  const enrichedItems = items.map((item) => ({
    color: item.color || 'black',
    ...item,
  }));
  const payload = JSON.stringify({
    cart: {
      userName,
      items: enrichedItems,
    },
  });

  const res = http.post(`${baseUrl}/basket`, payload, buildHeaders());
  check(res, {
    'basket stored': (r) => r.status === 201,
  }) || fail(`Failed to store basket: ${res.status} - ${res.body}`);
  return res.json();
}

export function getBasket(userName) {
  const baseUrl = getBaseUrl('basket');
  return http.get(`${baseUrl}/basket/${userName}`, buildHeaders());
}

export function deleteBasket(userName) {
  const baseUrl = getBaseUrl('basket');
  return http.del(`${baseUrl}/basket/${userName}`, null, buildHeaders());
}

export function checkoutBasket(checkoutPayload) {
  const baseUrl = getBaseUrl('basket');
  const res = http.post(`${baseUrl}/basket/checkout`, JSON.stringify({ basketCheckoutDto: checkoutPayload }), buildHeaders());
  check(res, {
    'checkout processed': (r) => r.status === 200 || r.status === 201,
  }) || fail(`Checkout failed: ${res.status} - ${res.body}`);
  return res.json();
}

export function createOrder(orderPayload) {
  const baseUrl = getBaseUrl('ordering');
  const res = http.post(`${baseUrl}/orders`, JSON.stringify({ order: orderPayload }), buildHeaders());
  check(res, {
    'order created': (r) => r.status === 201,
  }) || fail(`Order creation failed: ${res.status} - ${res.body}`);
  return res.json();
}

export function updateOrder(orderPayload) {
  const baseUrl = getBaseUrl('ordering');
  const res = http.put(`${baseUrl}/orders`, JSON.stringify({ order: orderPayload }), buildHeaders());
  check(res, {
    'order updated': (r) => r.status === 200,
  });
  return res.json();
}

export function deleteOrder(orderId) {
  const baseUrl = getBaseUrl('ordering');
  return http.del(`${baseUrl}/orders/${orderId}`, null, buildHeaders());
}

export function getOrders() {
  const baseUrl = getBaseUrl('ordering');
  return http.get(`${baseUrl}/orders`, buildHeaders());
}

export function getOrdersByName(orderName) {
  const baseUrl = getBaseUrl('ordering');
  return http.get(`${baseUrl}/orders/${orderName}`, buildHeaders());
}

export function getOrdersByCustomer(customerId) {
  const baseUrl = getBaseUrl('ordering');
  return http.get(`${baseUrl}/orders/customer/${customerId}`, buildHeaders());
}

export function randomPrice() {
  return Math.round(Math.random() * 10000) / 100 + 1;
}

export function buildOrderPayload({ userId, userName, productId, productPrice }) {
  const orderNumber = `ORDER-${Date.now()}-${Math.floor(Math.random() * 1000)}`;
  return {
    orderName: orderNumber,
    customerId: userId || uuidv4(),
    customerName: userName || defaults.testUserName,
    shippingAddress: {
      firstName: 'Test',
      lastName: 'User',
      emailAddress: defaults.testUserEmail,
      addressLine: '123 Test St',
      country: 'Testland',
      state: 'TS',
      zipCode: '12345',
    },
    billingAddress: {
      firstName: 'Test',
      lastName: 'User',
      emailAddress: defaults.testUserEmail,
      addressLine: '123 Test St',
      country: 'Testland',
      state: 'TS',
      zipCode: '12345',
    },
    payment: {
      cardName: 'Load Tester',
      cardNumber: '4111111111111111',
      expiration: '12/30',
      cvv: '123',
      paymentMethod: 1,
    },
    orderItems: [
      {
        productId,
        productName: 'Load Test Item',
        price: productPrice || 10,
        quantity: 1,
      },
    ],
  };
}

export function wait(min = 1, max = 3) {
  sleep(think(min, max));
}

export function pickOne(arr) {
  return randomItem(arr);
}
