import { loadTestOptions } from '../shared/testTypes.js';
import { defaults } from '../shared/config.js';
import { registerUser } from '../shared/helpers.js';
import { runAuthenticationLoad } from './user.scenario.js';

export const options = loadTestOptions({ tags: { service: 'user', testType: 'load' } });

export function setup() {
  const email = defaults.testUserEmail;
  registerUser({
    firstName: 'Login',
    lastName: 'Load',
    email,
    password: defaults.testUserPassword,
  });
  return { email, password: defaults.testUserPassword };
}

export default function (data) {
  runAuthenticationLoad(data);
}
