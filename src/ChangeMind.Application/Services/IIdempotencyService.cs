namespace ChangeMind.Application.Services;

using ChangeMind.Application.UseCases.Payments.Commands;

public interface IIdempotencyService
{
    Task<IdempotencyCheckResult> CheckAsync(Guid userId, Guid idempotencyKey, CancellationToken cancellationToken = default);
    Task CommitAsync(Guid userId, Guid idempotencyKey, PaymentProcessResponse response, CancellationToken cancellationToken = default);
}

public sealed record IdempotencyCheckResult(
    IdempotencyStatus Status,
    PaymentProcessResponse? CachedResponse);

public enum IdempotencyStatus
{
    /// <summary>Lock acquired — proceed with handler.</summary>
    New,
    /// <summary>Another request is currently processing this key — return 409 + Retry-After.</summary>
    InFlight,
    /// <summary>Response already cached — return cached body, skip handler.</summary>
    Duplicate,
    /// <summary>Redis unavailable — fail-open, DB constraint is the last guard.</summary>
    RedisUnavailable
}
