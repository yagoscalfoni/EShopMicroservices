#!/usr/bin/env bash
set -euo pipefail

if [ "$#" -lt 1 ]; then
  echo "Usage: $0 <k6-script> [k6 args...]" >&2
  exit 1
fi

SCRIPT="$1"
shift

RUN_ID="${RUN_ID:-$(date +%Y%m%d%H%M%S)}"
SERVICE_TAG="${SERVICE:-${service:-general}}"
TEST_TYPE_TAG="${TEST_TYPE:-${test_type:-adhoc}}"
INFLUX_HOST="${INFLUXDB_HOST:-localhost}"
INFLUX_PORT="${INFLUXDB_PORT:-8086}"
INFLUX_DB="${INFLUXDB_DB:-k6}"
INFLUX_USER="${INFLUXDB_USER:-k6}"
INFLUX_PASSWORD="${INFLUXDB_USER_PASSWORD:-k6pass}"

export K6_OUT="${K6_OUT:-influxdb=http://${INFLUX_USER}:${INFLUX_PASSWORD}@${INFLUX_HOST}:${INFLUX_PORT}/${INFLUX_DB}}"

k6 run \
  --tag run_id="${RUN_ID}" \
  --tag service="${SERVICE_TAG}" \
  --tag test_type="${TEST_TYPE_TAG}" \
  "$SCRIPT" "$@"
