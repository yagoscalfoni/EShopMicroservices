import { spikeTestOptions } from '../shared/testTypes.js';
import { defaults } from '../shared/config.js';
import { registerUser } from '../shared/helpers.js';
import { runAuthenticationLoad } from './user.scenario.js';

export const options = spikeTestOptions({ tags: { service: 'user', testType: 'spike' } });

export function setup() {
  const email = defaults.testUserEmail;
  registerUser({
    firstName: 'Login',
    lastName: 'Spike',
    email,
    password: defaults.testUserPassword,
  });
  return { email, password: defaults.testUserPassword };
}

export default function (data) {
  runAuthenticationLoad(data);
}
