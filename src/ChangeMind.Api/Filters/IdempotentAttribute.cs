namespace ChangeMind.Api.Filters;

using System.Security.Claims;
using ChangeMind.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Method)]
public sealed class IdempotentAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var headerValue)
            || string.IsNullOrWhiteSpace(headerValue))
        {
            context.Result = new BadRequestObjectResult("Idempotency-Key header is required.");
            return;
        }

        if (!Guid.TryParse(headerValue, out var idempotencyKey))
        {
            context.Result = new BadRequestObjectResult("Idempotency-Key must be a valid GUID.");
            return;
        }

        var userIdClaim = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var idempotencyService = context.HttpContext.RequestServices
            .GetRequiredService<IIdempotencyService>();

        var cancellationToken = context.HttpContext.RequestAborted;
        var check = await idempotencyService.CheckAsync(userId, idempotencyKey, cancellationToken);

        switch (check.Status)
        {
            case IdempotencyStatus.Duplicate:
                context.HttpContext.Response.Headers["X-Idempotent-Replayed"] = "true";
                context.Result = new OkObjectResult(check.CachedResponse);
                return;

            case IdempotencyStatus.InFlight:
                context.HttpContext.Response.Headers["Retry-After"] = "5";
                context.Result = new ConflictObjectResult("A request with this idempotency key is already being processed.");
                return;

            case IdempotencyStatus.New:
            case IdempotencyStatus.RedisUnavailable:
                context.HttpContext.Items["IdempotencyKey"] = idempotencyKey;
                context.HttpContext.Items["IdempotencyUserId"] = userId;
                context.HttpContext.Items["IdempotencyStatus"] = check.Status;
                break;
        }

        var executed = await next();

        if (executed.Exception is null && check.Status == IdempotencyStatus.New
            && executed.Result is OkObjectResult { Value: Application.UseCases.Payments.Commands.PaymentProcessResponse response })
        {
            await idempotencyService.CommitAsync(userId, idempotencyKey, response, cancellationToken);
        }
    }
}
