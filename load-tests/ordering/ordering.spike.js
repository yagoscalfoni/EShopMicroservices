import orderingFlow, { setupOrderingData, teardownOrderingData } from './ordering.scenario.js';
import { spikeTestOptions } from '../shared/testTypes.js';

export const options = spikeTestOptions({ tags: { service: 'ordering', testType: 'spike' } });

export function setup() {
  return setupOrderingData();
}

export default function (data) {
  orderingFlow(data);
}

export function teardown(data) {
  teardownOrderingData(data);
}
