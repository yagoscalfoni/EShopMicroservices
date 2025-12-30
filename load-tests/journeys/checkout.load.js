import checkoutJourney, { setupCheckoutJourney } from './checkout.scenario.js';
import { loadTestOptions } from '../shared/testTypes.js';

export const options = loadTestOptions({ tags: { service: 'journey', testType: 'load' }, target: 15, duration: '8m' });

export function setup() {
  return setupCheckoutJourney();
}

export default function (data) {
  checkoutJourney(data);
}
