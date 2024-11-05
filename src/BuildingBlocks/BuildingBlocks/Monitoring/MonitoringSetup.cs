using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Serilog;

namespace BuildingBlocks.Monitoring
{
    public static class MonitoringSetup
    {
        public static void AddMonitoring(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do Serilog para logs estruturados no console
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()  // Envia logs para o console
                .CreateLogger();

            // Adiciona Serilog como o logger da aplicação
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
