using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Capture ou inicie uma nova Activity para garantir a propagação do tracing
        var activity = Activity.Current ?? new Activity("LoggingBehavior").Start();

        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.TotalSeconds > 3) // Ajuste para precisão
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                typeof(TRequest).Name, timeTaken.TotalSeconds);
        }

        logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);

        // Pare a Activity se ela tiver sido iniciada localmente
        if (activity.OperationName == "LoggingBehavior")
            activity.Stop();

        return response;
    }
}
