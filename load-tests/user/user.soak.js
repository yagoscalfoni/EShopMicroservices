import { soakTestOptions } from '../shared/testTypes.js';
import { defaults } from '../shared/config.js';
import { registerUser } from '../shared/helpers.js';
import { runAuthenticationLoad } from './user.scenario.js';

export const options = soakTestOptions({ tags: { service: 'user', testType: 'soak' } });

export function setup() {
  const email = defaults.testUserEmail;
  registerUser({
    firstName: 'Login',
    lastName: 'Soak',
    email,
    password: defaults.testUserPassword,
  });
  return { email, password: defaults.testUserPassword };
}

export default function (data) {
  runAuthenticationLoad(data);
}
