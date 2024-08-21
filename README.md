# .NET Deployment

This project demonstrates the deployment environment of a .NET application.

The project utilizes the following libraries and tools:

- [OpenTelemetry](https://opentelemetry.io/)
- [GitHub Actions](https://docs.github.com/en/actions)
- [Prometheus](https://prometheus.io/docs/prometheus/latest/getting_started/)
    - [Alert Rules](https://prometheus.io/docs/prometheus/latest/configuration/alerting_rules/)
    - [Alertmanager](https://prometheus.io/docs/alerting/latest/overview/)
    - [Grafana](https://grafana.com/)
        - [Grafana Loki](https://grafana.com/oss/loki/)
        - [Grafana Tempo](https://grafana.com/oss/tempo/)
- [Kubernetes (K8S)](https://kubernetes.io/docs/concepts/)
- [Docker](https://docs.docker.com/compose/)
- [Helm Charts](https://helm.sh/)

### Prerequisites

* [Install & start Docker Desktop](https://docs.docker.com/engine/install/)

### Running 

#### Run the application with the following commands:

```
    cd etc/charts/app-demo-chart
    helm install [app_chart_name] .
```

#### Run Grafana
To install Grafana:
```
helm install grafana grafana/grafana -f value-override.yaml -n otel-demo
```

#### Run Grafana Loki

To install Loki:
```
helm install --values values.yaml loki grafana/loki -n otel-demo
```

#### Run Prometheus

To install Prometheus:
```
helm install prometheus-demo prometheus-community/prometheus -f values-override.yaml -n otel-demo
```

#### Run OpenTelemetry Collector
```
helm install opentelemetry-collector open-telemetry/opentelemetry-collector -f values-override.yaml -n otel-demo
```

#### Run Grafana Tempo

```
helm install tempo grafana/tempo -n otel-demo
```

### Contributing

I welcome contributions to this project! Feel free to open pull requests with improvements, bug fixes, or additional features.

