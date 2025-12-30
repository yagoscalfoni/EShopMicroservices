import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

export const defaults = {
  gatewayBaseUrl: __ENV.GATEWAY_BASE_URL || 'http://localhost:6004',
  catalogBaseUrl: __ENV.CATALOG_BASE_URL || 'http://localhost:6000',
  basketBaseUrl: __ENV.BASKET_BASE_URL || 'http://localhost:6001',
  orderingBaseUrl: __ENV.ORDERING_BASE_URL || 'http://localhost:6003',
  userBaseUrl: __ENV.USER_BASE_URL || 'http://localhost:6006',
  testUserEmail: __ENV.TEST_USER_EMAIL || `loadtest-user-${Date.now()}@example.com`,
  testUserPassword: __ENV.TEST_USER_PASSWORD || 'P@ssword123!',
  testUserName: __ENV.TEST_USER_NAME || 'Load Tester',
};

export const thresholds = {
  smoke: {
    http_req_failed: ['rate<0.01'],
    http_req_duration: ['p(95)<750'],
  },
  load: {
    http_req_failed: ['rate<0.02'],
    http_req_duration: ['p(95)<1000', 'p(99)<1500'],
  },
  stress: {
    http_req_failed: ['rate<0.05'],
    http_req_duration: ['p(95)<1500', 'p(99)<2500'],
  },
  spike: {
    http_req_failed: ['rate<0.10'],
    http_req_duration: ['p(95)<2000', 'p(99)<3000'],
  },
  soak: {
    http_req_failed: ['rate<0.05'],
    http_req_duration: ['p(95)<1200', 'p(99)<2000'],
  },
};

export function getBaseUrl(service) {
  const useGateway = __ENV.USE_GATEWAY === 'true';
  if (useGateway) {
    return defaults.gatewayBaseUrl;
  }
  switch (service) {
    case 'catalog':
      return defaults.catalogBaseUrl;
    case 'basket':
      return defaults.basketBaseUrl;
    case 'ordering':
      return defaults.orderingBaseUrl;
    case 'user':
      return defaults.userBaseUrl;
    default:
      return defaults.gatewayBaseUrl;
  }
}

export function think(minSeconds = 1, maxSeconds = 3) {
  const delay = randomIntBetween(minSeconds * 1000, maxSeconds * 1000) / 1000;
  return delay;
}
