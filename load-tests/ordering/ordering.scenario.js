import { check } from 'k6';
import { defaults } from '../shared/config.js';
import {
  registerUser,
  createProduct,
  buildOrderPayload,
  createOrder,
  updateOrder,
  deleteOrder,
  getOrders,
  getOrdersByCustomer,
  getOrdersByName,
  wait,
} from '../shared/helpers.js';

export function setupOrderingData() {
  const registration = registerUser({
    firstName: 'Order',
    lastName: 'Runner',
    email: defaults.testUserEmail,
    password: defaults.testUserPassword,
  });

  const product = createProduct({ category: ['orders'] });
  const orderPayload = buildOrderPayload({
    userId: registration.id,
    userName: registration.email,
    productId: product.id,
    productPrice: product.price,
  });
  const createdOrder = createOrder(orderPayload);

  return {
    userId: registration.id,
    baseOrderId: createdOrder.id,
    baseOrderName: orderPayload.orderName,
    productId: product.id,
    productPrice: product.price,
  };
}

export function teardownOrderingData(data) {
  if (data && data.baseOrderId) {
    deleteOrder(data.baseOrderId);
  }
}

export default function orderingFlow(data) {
  const orders = getOrders();
  check(orders, { 'orders list 200': (r) => r.status === 200 });
  wait();

  // Customer-centric lookup is key for account order history pages
  const ordersByCustomer = getOrdersByCustomer(data.userId);
  check(ordersByCustomer, { 'orders by customer 200': (r) => r.status === 200 });
  wait();

  // Order detail retrieval by friendly name for CS/support tooling
  const byName = getOrdersByName(data.baseOrderName);
  check(byName, { 'orders by name 200': (r) => r.status === 200 });
  wait();

  const newOrderPayload = buildOrderPayload({
    userId: data.userId,
    userName: defaults.testUserName,
    productId: data.productId,
    productPrice: data.productPrice,
  });
  const newOrder = createOrder(newOrderPayload);
  wait();

  // Update simulates post-purchase edits (address changes, etc.)
  newOrderPayload.orderName = `${newOrderPayload.orderName}-updated`;
  newOrderPayload.id = newOrder.id;
  updateOrder(newOrderPayload);
  wait();

  // Deleting test orders keeps the dataset lean for repeated runs
  deleteOrder(newOrder.id);
  wait();
}
