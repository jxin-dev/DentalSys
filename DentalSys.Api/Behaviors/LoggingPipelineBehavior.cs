using MediatR;
using System.Diagnostics;

namespace DentalSys.Api.Behaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation($"Handling { requestName }");
            var stopwatch = Stopwatch.StartNew();

            var response = await next();
            
            stopwatch.Stop();
            _logger.LogInformation($"Handled { requestName } in {stopwatch.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
