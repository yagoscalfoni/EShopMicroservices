import { thresholds } from './config.js';

export function smokeTestOptions({ tags = {} } = {}) {
  return {
    vus: 1,
    duration: '1m',
    thresholds: thresholds.smoke,
    tags,
  };
}

export function loadTestOptions({ target = 20, duration = '5m', tags = {} } = {}) {
  return {
    vus: Math.min(target, 50),
    duration,
    thresholds: thresholds.load,
    tags,
  };
}

export function stressTestOptions({ start = 10, peak = 80, tags = {} } = {}) {
  return {
    stages: [
      { duration: '2m', target: start },
      { duration: '5m', target: peak },
      { duration: '3m', target: 0 },
    ],
    thresholds: thresholds.stress,
    tags,
  };
}

export function spikeTestOptions({ spikeTo = 120, settle = 10, tags = {} } = {}) {
  return {
    stages: [
      { duration: '30s', target: settle },
      { duration: '10s', target: spikeTo },
      { duration: '1m', target: settle },
      { duration: '1m', target: 0 },
    ],
    thresholds: thresholds.spike,
    tags,
  };
}

export function soakTestOptions({ steady = 20, duration = '30m', tags = {} } = {}) {
  return {
    vus: steady,
    duration,
    thresholds: thresholds.soak,
    tags,
  };
}
