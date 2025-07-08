using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Serilog;
using System;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Monitoring
{
    public static class MonitoringSetup
    {
        public static void AddMonitoring(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do Serilog para logs estruturados no console
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console() // Envia logs para o console
                .CreateLogger();

            // Adiciona Serilog como o logger da aplicação
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            // Configuração de recurso compartilhada para OpenTelemetry
            var resource = ResourceBuilder.CreateDefault()
                .AddService(serviceName:"User.API", serviceVersion: "1.0.0");

            // Configuração do OpenTelemetry para traces
            services.AddOpenTelemetry().WithTracing(traceBuilder =>
            {
                traceBuilder
                    .SetResourceBuilder(resource)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(configuration.GetValue("Otlp:Endpoint", "http://localhost:4317"));
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    })
                    .AddConsoleExporter();
            });

            // Configuração do OpenTelemetry para métricas
            services.AddOpenTelemetry().WithMetrics(metricProviderBuilder =>
            {
                metricProviderBuilder
                    .SetResourceBuilder(resource)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter();
            });

            // Configuração do OpenTelemetry para logs com Console e OTLP
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resource);
                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;
                    options.ParseStateValues = true;
                    options.AttachLogsToActivityEvent();
                    options.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(configuration.GetValue("Otlp:Endpoint", "http://localhost:4317"));
                        otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
                    options.AddConsoleExporter();
                });
            });
        }
    }
}
