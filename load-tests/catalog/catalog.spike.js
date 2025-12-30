import catalogFlow, { setupCatalogData, teardownCatalogData } from './catalog.scenario.js';
import { spikeTestOptions } from '../shared/testTypes.js';

export const options = spikeTestOptions({ tags: { service: 'catalog', testType: 'spike' } });

export function setup() {
  return setupCatalogData();
}

export default function (data) {
  catalogFlow(data);
}

export function teardown(data) {
  teardownCatalogData(data);
}
