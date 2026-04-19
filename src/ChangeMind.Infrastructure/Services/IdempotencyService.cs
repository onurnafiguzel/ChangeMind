namespace ChangeMind.Infrastructure.Services;

using System.Text.Json;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Services;
using ChangeMind.Application.UseCases.Payments.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;
using StackExchange.Redis;

public sealed class IdempotencyService(
    IOptions<CacheOptions> cacheOptions,
    IOptions<IdempotencyOptions> options,
    ILogger<IdempotencyService> logger) : IIdempotencyService, IAsyncDisposable
{
    private readonly IdempotencyOptions _opts = options.Value;
    private readonly ConnectionMultiplexer _redis = CreateConnection(cacheOptions.Value);

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // Lua script: atomically delete the lock and write the long-lived response cache.
    // Eliminates the race window between lock release and response cache write.
    private const string CommitScript = """
        local lock = KEYS[1]
        local resp  = KEYS[2]
        local val   = ARGV[1]
        local ttl   = ARGV[2]
        redis.call('DEL', lock)
        redis.call('SET', resp, val, 'PX', ttl)
        return 1
        """;

    // LoggerMessage.Define — zero-allocation (CA1848)
    private static readonly Action<ILogger, Guid, Guid, Exception?> _logLockAcquired =
        LoggerMessage.Define<Guid, Guid>(LogLevel.Information, new EventId(10, "LockAcquired"),
            "Idempotency lock acquired. UserId={UserId} Key={Key}");

    private static readonly Action<ILogger, Guid, Guid, Exception?> _logDuplicate =
        LoggerMessage.Define<Guid, Guid>(LogLevel.Information, new EventId(11, "Duplicate"),
            "Idempotency duplicate detected via cache. UserId={UserId} Key={Key}");

    private static readonly Action<ILogger, Guid, Guid, Exception?> _logInFlight =
        LoggerMessage.Define<Guid, Guid>(LogLevel.Warning, new EventId(12, "InFlight"),
            "Idempotency in-flight conflict. UserId={UserId} Key={Key}");

    private static readonly Action<ILogger, Guid, Guid, Exception?> _logRedisUnavailable =
        LoggerMessage.Define<Guid, Guid>(LogLevel.Warning, new EventId(13, "RedisUnavailable"),
            "Idempotency Redis unavailable — failing open. UserId={UserId} Key={Key}");

    private static readonly Action<ILogger, Guid, Guid, int, Exception?> _logCommitted =
        LoggerMessage.Define<Guid, Guid, int>(LogLevel.Information, new EventId(14, "Committed"),
            "Idempotency response cached. UserId={UserId} Key={Key} TtlHours={TtlHours}");

    private static readonly Action<ILogger, Guid, Guid, Exception?> _logCommitFailed =
        LoggerMessage.Define<Guid, Guid>(LogLevel.Error, new EventId(15, "CommitFailed"),
            "Idempotency commit failed after payment success. UserId={UserId} Key={Key}");

    public async Task<IdempotencyCheckResult> CheckAsync(
        Guid userId, Guid idempotencyKey, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var lockKey     = LockKey(userId, idempotencyKey);
        var responseKey = ResponseKey(userId, idempotencyKey);

        try
        {
            // Phase 1: check if a completed response is already cached
            var cached = await db.StringGetAsync(responseKey);
            if (!cached.IsNullOrEmpty)
            {
                var response = JsonSerializer.Deserialize<PaymentProcessResponse>((string)cached!, JsonOpts)!;
                _logDuplicate(logger, userId, idempotencyKey, null);
                return new IdempotencyCheckResult(IdempotencyStatus.Duplicate, response);
            }

            // Phase 2: try to acquire the in-flight distributed lock (NX = only if not exists)
            var acquired = await db.StringSetAsync(
                lockKey, "processing",
                TimeSpan.FromMilliseconds(_opts.LockTtlMs),
                When.NotExists);

            if (!acquired)
            {
                _logInFlight(logger, userId, idempotencyKey, null);
                return new IdempotencyCheckResult(IdempotencyStatus.InFlight, null);
            }

            _logLockAcquired(logger, userId, idempotencyKey, null);
            return new IdempotencyCheckResult(IdempotencyStatus.New, null);
        }
        catch (Exception ex) when (ex is RedisException or BrokenCircuitException)
        {
            _logRedisUnavailable(logger, userId, idempotencyKey, ex);
            return new IdempotencyCheckResult(IdempotencyStatus.RedisUnavailable, null);
        }
    }

    public async Task CommitAsync(
        Guid userId, Guid idempotencyKey, PaymentProcessResponse response, CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var lockKey     = LockKey(userId, idempotencyKey);
        var responseKey = ResponseKey(userId, idempotencyKey);
        var ttlMs       = (long)TimeSpan.FromHours(_opts.ResponseTtlHours).TotalMilliseconds;

        try
        {
            var serialized = JsonSerializer.Serialize(response, JsonOpts);
            await db.ScriptEvaluateAsync(
                CommitScript,
                keys: [lockKey, responseKey],
                values: [serialized, ttlMs]);

            _logCommitted(logger, userId, idempotencyKey, _opts.ResponseTtlHours, null);
        }
        catch (Exception ex) when (ex is RedisException or BrokenCircuitException)
        {
            // Payment already succeeded — do not rethrow. Next retry will hit the DB constraint.
            _logCommitFailed(logger, userId, idempotencyKey, ex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _redis.DisposeAsync();
    }

    private static ConnectionMultiplexer CreateConnection(CacheOptions opts) =>
        ConnectionMultiplexer.Connect(opts.ConnectionString);

    private static string LockKey(Guid userId, Guid key)     => $"changemind:idempotency:lock:{userId}:{key}";
    private static string ResponseKey(Guid userId, Guid key) => $"changemind:idempotency:response:{userId}:{key}";
}
