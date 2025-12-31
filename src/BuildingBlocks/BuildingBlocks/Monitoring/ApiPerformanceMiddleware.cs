using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Monitoring;

public class ApiPerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Histogram<double> _durationHistogram;
    private readonly ILogger<ApiPerformanceMiddleware> _logger;

    public ApiPerformanceMiddleware(RequestDelegate next, IMeterFactory meterFactory, ILogger<ApiPerformanceMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        var meter = meterFactory.Create("User.API");
        _durationHistogram = meter.CreateHistogram<double>(
            name: "api.request.duration",
            unit: "ms",
            description: "Tempo de resposta das APIs por rota");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            var tags = new TagList
            {
                { "http.method", context.Request.Method },
                { "http.route", context.GetEndpoint()?.DisplayName ?? context.Request.Path.ToString() },
                { "http.status_code", context.Response?.StatusCode ?? 0 }
            };

            _durationHistogram.Record(stopwatch.Elapsed.TotalMilliseconds, tags);
            _logger.LogInformation(
                "API {Method} {Path} respondeu {StatusCode} em {Elapsed:F2} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response?.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}

public static class ApiPerformanceMiddlewareExtensions
{
    public static IApplicationBuilder UseApiPerformanceMonitoring(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiPerformanceMiddleware>();
    }
}
