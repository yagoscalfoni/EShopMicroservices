import basketFlow, { setupBasketData, teardownBasketData } from './basket.scenario.js';
import { spikeTestOptions } from '../shared/testTypes.js';

export const options = spikeTestOptions({ tags: { service: 'basket', testType: 'spike' } });

export function setup() {
  return setupBasketData();
}

export default function (data) {
  basketFlow(data);
}

export function teardown(data) {
  teardownBasketData(data);
}
