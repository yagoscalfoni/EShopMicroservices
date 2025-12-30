import basketFlow, { setupBasketData, teardownBasketData } from './basket.scenario.js';
import { soakTestOptions } from '../shared/testTypes.js';

export const options = soakTestOptions({ tags: { service: 'basket', testType: 'soak' } });

export function setup() {
  return setupBasketData();
}

export default function (data) {
  basketFlow(data);
}

export function teardown(data) {
  teardownBasketData(data);
}
