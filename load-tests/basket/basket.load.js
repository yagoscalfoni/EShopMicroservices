import basketFlow, { setupBasketData, teardownBasketData } from './basket.scenario.js';
import { loadTestOptions } from '../shared/testTypes.js';

export const options = loadTestOptions({ tags: { service: 'basket', testType: 'load' } });

export function setup() {
  return setupBasketData();
}

export default function (data) {
  basketFlow(data);
}

export function teardown(data) {
  teardownBasketData(data);
}
