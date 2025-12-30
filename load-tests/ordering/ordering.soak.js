import orderingFlow, { setupOrderingData, teardownOrderingData } from './ordering.scenario.js';
import { soakTestOptions } from '../shared/testTypes.js';

export const options = soakTestOptions({ tags: { service: 'ordering', testType: 'soak' } });

export function setup() {
  return setupOrderingData();
}

export default function (data) {
  orderingFlow(data);
}

export function teardown(data) {
  teardownOrderingData(data);
}
