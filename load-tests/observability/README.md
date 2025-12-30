# k6 Observability Stack

A Docker Compose stack that centralizes k6 load test metrics in InfluxDB and surfaces real-time and historical insights in Grafana. It is intentionally minimal, production-leaning, and safe to publish.

## Architecture
- **InfluxDB 1.8** — time-series backend for k6 metrics with authentication enabled and persistent volume storage.
- **Grafana** — pre-provisioned datasource and dashboards to visualize throughput, latency (avg/p95/p99), error rates, endpoint trends, and run-to-run comparisons via `run_id` tags.
- **Named volumes** — `influxdb-data`, `grafana-data` persist data across restarts for historical views.

## Getting started
1. Copy and adjust credentials (never commit secrets):
   ```bash
   cp load-tests/observability/.env.observability.example load-tests/observability/.env.observability
   # edit credentials as needed
   ```
2. Start the observability stack:
   ```bash
   docker compose -f load-tests/observability/docker-compose.observability.yml \
     --env-file load-tests/observability/.env.observability up -d
   ```
3. Run any k6 script with InfluxDB output enabled and rich tagging for filtering:
   ```bash
   # Example: catalog load test with tagging for service, test type, and a unique run id
   K6_OUT=influxdb=http://${INFLUXDB_USER:-k6}:${INFLUXDB_USER_PASSWORD:-k6pass}@localhost:8086/${INFLUXDB_DB:-k6} \
   k6 run --tag service=catalog --tag test_type=load --tag run_id=$(date +%Y%m%d%H%M%S) load-tests/catalog/catalog.load.js
   ```
   - Use `--tag scenario=<scenarioName>` and `--tag flow=<businessFlow>` if you want to distinguish specific journeys.
   - `K6_OUT` can also be set globally in the environment instead of per-command.
4. Open Grafana at `http://localhost:3000` (credentials from `.env.observability`):
   - Datasource: `k6-influxdb` is auto-configured.
   - Dashboard: **k6 Load Tests - Overview** is pre-loaded under the "k6 Load Tests" folder.

## Dashboards and interpretation
- **Requests per second & latency cards** — quickly confirm whether throughput and latency match expected profiles for the chosen test type.
- **Latency percentiles over time** — watch for tail latency spikes under stress/spike runs.
- **Error rate & status distribution** — isolate failing endpoints and HTTP status anomalies.
- **Endpoint drilldowns** — tables highlight the slowest and noisiest endpoints (by `name` tag) to focus tuning.
- **Scenario throughput** — shows how each scripted scenario performs under load, leveraging the built-in `scenario` tag from k6.
- **Run comparison** — filter by `run_id` to compare releases or configuration changes.

## Data hygiene and safety
- Keep this stack pointed at non-production environments; production credentials should be injected via CI/CD secrets.
- Named volumes retain metrics for historical comparison; prune with `docker volume rm` if you need to reset.
- Do not commit `.env.observability`—use the provided `.example` file as a template.

## Automation helper (optional)
Use the helper script to standardize tagging and output when running locally or in CI:
```bash
./load-tests/observability/run-with-influx.sh load-tests/ordering/ordering.stress.js --vus 50 --duration 5m \
  --tag service=ordering --tag test_type=stress
```
The script defaults `run_id` to a timestamp if you do not supply one and wires `K6_OUT` with the credentials/host values from your environment.
