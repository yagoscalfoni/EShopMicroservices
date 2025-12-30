import http from 'k6/http';
import { check } from 'k6';
import { createProduct, deleteProduct, updateProduct, getProductById, browseCatalog, pickOne, wait } from '../shared/helpers.js';
import { getBaseUrl } from '../shared/config.js';

export function setupCatalogData() {
  const created = createProduct({ category: ['load-test', 'summer'] });
  return { productId: created.id, category: pickOne(['load-test', 'summer']) };
}

export function teardownCatalogData(data) {
  if (data && data.productId) {
    deleteProduct(data.productId);
  }
}

export default function catalogFlow(data) {
  const baseUrl = getBaseUrl('catalog');
  const category = data.category;

  // Browsing and filtering mimic user discovery traffic
  const [listRes, categoryRes] = browseCatalog({ category });
  check(listRes, { 'list products 200': (r) => r.status === 200 });
  check(categoryRes, { 'category products 200': (r) => r.status === 200 });
  wait();

  // Product detail lookups are latency-sensitive for PDP rendering
  const productRes = getProductById(data.productId);
  check(productRes, { 'product by id 200': (r) => r.status === 200 });
  wait();

  // Admin/test-data endpoints are covered with ephemeral products
  const created = createProduct({ namePrefix: 'Transient Catalog Item', category: ['transient'] });
  const updatePayload = {
    id: created.id,
    name: `${created.name || 'Transient Catalog Item'} - updated`,
    category: ['transient'],
    description: 'Updated during load test run',
    imageFile: 'placeholder.png',
    price: (created.price || 10) + 1,
  };
  updateProduct(created.id, updatePayload);
  wait();

  deleteProduct(created.id);
  wait();

  // Lightweight pagination probe
  const paged = http.get(`${baseUrl}/products?pageNumber=1&pageSize=5`);
  check(paged, { 'paged products 200': (r) => r.status === 200 });
}
