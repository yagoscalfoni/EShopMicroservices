import { smokeTestOptions } from '../shared/testTypes.js';
import { runRegistrationSmoke } from './user.scenario.js';

export const options = smokeTestOptions({ tags: { service: 'user', testType: 'smoke' } });

export default function () {
  runRegistrationSmoke();
}
