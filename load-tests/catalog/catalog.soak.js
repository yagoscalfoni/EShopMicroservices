import catalogFlow, { setupCatalogData, teardownCatalogData } from './catalog.scenario.js';
import { soakTestOptions } from '../shared/testTypes.js';

export const options = soakTestOptions({ tags: { service: 'catalog', testType: 'soak' } });

export function setup() {
  return setupCatalogData();
}

export default function (data) {
  catalogFlow(data);
}

export function teardown(data) {
  teardownCatalogData(data);
}
