import orderingFlow, { setupOrderingData, teardownOrderingData } from './ordering.scenario.js';
import { stressTestOptions } from '../shared/testTypes.js';

export const options = stressTestOptions({ tags: { service: 'ordering', testType: 'stress' } });

export function setup() {
  return setupOrderingData();
}

export default function (data) {
  orderingFlow(data);
}

export function teardown(data) {
  teardownOrderingData(data);
}
