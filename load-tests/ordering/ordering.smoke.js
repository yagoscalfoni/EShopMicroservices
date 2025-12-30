import orderingFlow, { setupOrderingData, teardownOrderingData } from './ordering.scenario.js';
import { smokeTestOptions } from '../shared/testTypes.js';

export const options = smokeTestOptions({ tags: { service: 'ordering', testType: 'smoke' } });

export function setup() {
  return setupOrderingData();
}

export default function (data) {
  orderingFlow(data);
}

export function teardown(data) {
  teardownOrderingData(data);
}
