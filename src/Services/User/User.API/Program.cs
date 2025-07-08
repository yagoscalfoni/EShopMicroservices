using User.API;
using User.Application;
using User.Infrastructure;
using User.Infrastructure.Data.Extensions;
using BuildingBlocks.Monitoring;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços principais
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration)
    .AddMonitoring(builder.Configuration); // Configura o monitoramento centralizado

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o endpoint para o Prometheus coletar métricas
app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitialiseDatabaseAsync();
}

app.UseApiServices();
app.Run();
