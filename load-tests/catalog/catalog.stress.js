import catalogFlow, { setupCatalogData, teardownCatalogData } from './catalog.scenario.js';
import { stressTestOptions } from '../shared/testTypes.js';

export const options = stressTestOptions({ tags: { service: 'catalog', testType: 'stress' } });

export function setup() {
  return setupCatalogData();
}

export default function (data) {
  catalogFlow(data);
}

export function teardown(data) {
  teardownCatalogData(data);
}
