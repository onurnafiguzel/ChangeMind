namespace ChangeMind.Application.Behaviors;

using FluentValidation;
using MediatR;
using ValidationException = ChangeMind.Domain.Exceptions.ValidationException;

/// <summary>
/// MediatR pipeline behavior that runs all registered FluentValidation validators
/// for a request before it reaches the handler. Throws <see cref="ValidationException"/>
/// (HTTP 400) if any rule fails — handlers stay validation-free.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count > 0)
        {
            var errors = failures
                .Select(f => f.ErrorMessage)
                .ToList();

            throw new ValidationException(errors);
        }

        return await next();
    }
}
