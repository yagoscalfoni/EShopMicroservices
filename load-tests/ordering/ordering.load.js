import orderingFlow, { setupOrderingData, teardownOrderingData } from './ordering.scenario.js';
import { loadTestOptions } from '../shared/testTypes.js';

export const options = loadTestOptions({ tags: { service: 'ordering', testType: 'load' } });

export function setup() {
  return setupOrderingData();
}

export default function (data) {
  orderingFlow(data);
}

export function teardown(data) {
  teardownOrderingData(data);
}
