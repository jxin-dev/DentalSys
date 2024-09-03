using DentalSys.Api.Common;
using FluentValidation;
using MediatR;

namespace DentalSys.Api.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(validationFailure => validationFailure is not null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures.First().ErrorMessage);
            }

            return await next();
        }
    }
}
