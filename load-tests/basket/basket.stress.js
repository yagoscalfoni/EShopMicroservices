import basketFlow, { setupBasketData, teardownBasketData } from './basket.scenario.js';
import { stressTestOptions } from '../shared/testTypes.js';

export const options = stressTestOptions({ tags: { service: 'basket', testType: 'stress' } });

export function setup() {
  return setupBasketData();
}

export default function (data) {
  basketFlow(data);
}

export function teardown(data) {
  teardownBasketData(data);
}
