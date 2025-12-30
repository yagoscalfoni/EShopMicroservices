# k6 Load Testing Suite

This suite introduces production-oriented load tests for the microservice APIs using [k6](https://k6.io/). Tests are organized by business flow (catalog, basket, ordering, authentication) and by test type (smoke, load, stress, spike, soak). Each script is intentionally small, readable, and ready for local or CI execution.

## Base URLs and configuration
- Default service URLs point to Docker compose ports: Catalog `http://localhost:6000`, Basket `http://localhost:6001`, Ordering `http://localhost:6003`, User `http://localhost:6006`, Gateway `http://localhost:6004`.
- Override any service URL via environment variables: `CATALOG_BASE_URL`, `BASKET_BASE_URL`, `ORDERING_BASE_URL`, `USER_BASE_URL`, `GATEWAY_BASE_URL`.
- Set `USE_GATEWAY=true` to route every call through the API Gateway instead of service endpoints.
- Provide test credentials when reusing accounts: `TEST_USER_EMAIL`, `TEST_USER_PASSWORD`, `TEST_USER_NAME`.

## Business scenarios and endpoint coverage
- **Browse catalog**: `GET /products`, `GET /products/{id}`, `GET /products/category/{category}` — validates catalog read latency and pagination behavior.
- **Manage catalog items (admin/test data)**: `POST /products`, `PUT /products`, `DELETE /products/{id}` — exercised with synthetic products created and cleaned up by the tests to avoid polluting real catalog data.
- **Cart lifecycle**: `GET /basket/{userName}`, `POST /basket` (create/update cart), `DELETE /basket/{userName}`, `POST /basket/checkout` — mimics shoppers filling carts, updating quantities, and triggering checkout.
- **Ordering**: `GET /orders`, `GET /orders/{orderName}`, `GET /orders/customer/{customerId}`, `POST /orders`, `PUT /orders`, `DELETE /orders/{id}` — checks read and write load for downstream fulfillment.
- **Authentication**: `POST /register`, `POST /authenticate` — validates token issuance. Registration is kept to light usage to avoid excessive user creation; authentication is used heavily.

## Test types (when to run)
- **Smoke** — quick correctness on every build; low VUs, short duration.
- **Load** — expected steady traffic (baseline SLAs) for pre-release validation.
- **Stress** — find system breaking points by pushing past expected traffic.
- **Spike** — observe resilience to sudden traffic bursts (promo/email campaigns).
- **Soak** — long-running stability and resource leak detection.

## Data strategy
- Uses synthetic test users and products created during `setup` and cleaned in `teardown` where safe.
- Order creation and checkout run against isolated test data; run against non-production environments to avoid polluting real systems.
- Credentials and secrets are **never** hardcoded—only defaults for local dev. Override in CI via environment variables.

## Running tests
Each script is a standalone k6 entrypoint. Example commands:

```bash
# Smoke test catalog
k6 run load-tests/catalog/catalog.smoke.js

# Load test cart flow via gateway
USE_GATEWAY=true TEST_USER_EMAIL=tester@example.com TEST_USER_PASSWORD="P@ssword123!" \
  k6 run load-tests/basket/basket.load.js

# Full checkout journey stress test
k6 run load-tests/journeys/checkout.stress.js
```

## Observability and visualization
- A Docker-based stack under `load-tests/observability` provides InfluxDB for k6 metrics storage and Grafana for real-time and historical dashboards.
- Start the stack locally with: `docker compose -f load-tests/observability/docker-compose.observability.yml --env-file load-tests/observability/.env.observability up -d`.
- Send k6 results to InfluxDB by adding the output flag (example for catalog load test):
  ```bash
  K6_OUT=influxdb=http://k6:k6pass@localhost:8086/k6 \
  k6 run --tag run_id=$(date +%Y%m%d%H%M%S) --tag service=catalog --tag test_type=load load-tests/catalog/catalog.load.js
  ```
- Open Grafana at `http://localhost:3000` (credentials configurable via `.env.observability`) and use the **k6 Load Tests - Overview** dashboard to review throughput, latency percentiles, errors, and per-endpoint trends. Use the `run_id` filter to compare multiple executions.

## What we intentionally do NOT load test
- **Admin-only or low-frequency endpoints beyond the ones above**: the suite focuses on shopper-facing traffic. Admin APIs should be profiled separately with lower concurrency.
- **gRPC Discount service**: exposed internally; omitted from HTTP load to keep scope on public REST APIs.
