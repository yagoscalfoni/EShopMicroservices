import checkoutJourney, { setupCheckoutJourney } from './checkout.scenario.js';
import { stressTestOptions } from '../shared/testTypes.js';

export const options = stressTestOptions({ tags: { service: 'journey', testType: 'stress' }, start: 20, peak: 120 });

export function setup() {
  return setupCheckoutJourney();
}

export default function (data) {
  checkoutJourney(data);
}
