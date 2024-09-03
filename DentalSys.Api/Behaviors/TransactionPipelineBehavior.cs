using DentalSys.Api.Database;
using MediatR;

namespace DentalSys.Api.Behaviors
{
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly DentalDbContext _dbContext;

        public TransactionPipelineBehavior(DentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (IsTransactionRequest(request))
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var response = await next();

                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return response;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            else
            {
                return await next();
            }
        }

        private bool IsTransactionRequest(TRequest request)
        {
            var requestType = typeof(TRequest);
            return requestType.Name.Contains("Command", StringComparison.OrdinalIgnoreCase) &&
                !requestType.Namespace!.Contains("LoginUser", StringComparison.OrdinalIgnoreCase);
        }
    }
}
