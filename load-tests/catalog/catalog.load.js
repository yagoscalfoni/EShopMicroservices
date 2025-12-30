import catalogFlow, { setupCatalogData, teardownCatalogData } from './catalog.scenario.js';
import { loadTestOptions } from '../shared/testTypes.js';

export const options = loadTestOptions({ tags: { service: 'catalog', testType: 'load' } });

export function setup() {
  return setupCatalogData();
}

export default function (data) {
  catalogFlow(data);
}

export function teardown(data) {
  teardownCatalogData(data);
}
