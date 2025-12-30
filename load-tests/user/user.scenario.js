import { check } from 'k6';
import { defaults } from '../shared/config.js';
import { registerUser, authenticateUser, wait } from '../shared/helpers.js';

export function runRegistrationSmoke() {
  const registration = registerUser({
    firstName: 'Signup',
    lastName: 'Smoke',
    email: `smoke-${Date.now()}-${Math.floor(Math.random() * 1000)}@example.com`,
    password: defaults.testUserPassword,
  });
  wait();

  const auth = authenticateUser({ email: registration.email, password: defaults.testUserPassword });
  check(auth, { 'received token': (res) => !!res.token });
  wait();
}

export function runAuthenticationLoad(data) {
  const auth = authenticateUser({ email: data.email || defaults.testUserEmail, password: data.password || defaults.testUserPassword });
  check(auth, { 'auth token issued': (res) => !!res.token });
  wait();
}
