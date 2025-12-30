import basketFlow, { setupBasketData, teardownBasketData } from './basket.scenario.js';
import { smokeTestOptions } from '../shared/testTypes.js';

export const options = smokeTestOptions({ tags: { service: 'basket', testType: 'smoke' } });

export function setup() {
  return setupBasketData();
}

export default function (data) {
  basketFlow(data);
}

export function teardown(data) {
  teardownBasketData(data);
}
