import catalogFlow, { setupCatalogData, teardownCatalogData } from './catalog.scenario.js';
import { smokeTestOptions } from '../shared/testTypes.js';

export const options = smokeTestOptions({ tags: { service: 'catalog', testType: 'smoke' } });

export function setup() {
  return setupCatalogData();
}

export default function (data) {
  catalogFlow(data);
}

export function teardown(data) {
  teardownCatalogData(data);
}
