namespace ChangeMind.Infrastructure.Services;

using System.Text.Json;
using System.Threading.RateLimiting;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using StackExchange.Redis;

public sealed class RedisCacheService(
    IOptions<CacheOptions> options,
    ILogger<RedisCacheService> logger) : ICacheService, IAsyncDisposable
{
    private readonly CacheOptions _options = options.Value;
    private readonly ConnectionMultiplexer _connection = CreateConnection(options.Value);
    private readonly ResiliencePipeline _pipeline = BuildPipeline(options.Value.Resilience, logger);
    private readonly ConcurrencyLimiter _bulkhead = CreateBulkhead(options.Value.Resilience);

    private IDatabase Db => _connection.GetDatabase();

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // LoggerMessage.Define — zero-allocation, lazy-evaluated (CA1848 compliant)
    private static readonly Action<ILogger, string, string, Exception?> _logCircuitOpen =
        LoggerMessage.Define<string, string>(LogLevel.Warning, new EventId(1, "CircuitOpen"),
            "Redis circuit is open. {Operation} '{Key}' skipped.");

    private static readonly Action<ILogger, string, string, Exception?> _logOperationFailed =
        LoggerMessage.Define<string, string>(LogLevel.Warning, new EventId(2, "OperationFailed"),
            "Redis {Operation} failed for key '{Key}' after retries.");

    private static readonly Action<ILogger, int, int, double, string?, Exception?> _logRetry =
        LoggerMessage.Define<int, int, double, string?>(LogLevel.Warning, new EventId(3, "Retry"),
            "Redis retry {Attempt}/{Max} after {DelayMs}ms. Reason: {Message}");

    private static readonly Action<ILogger, int, string?, Exception?> _logCircuitOpened =
        LoggerMessage.Define<int, string?>(LogLevel.Error, new EventId(4, "CircuitOpened"),
            "Redis circuit OPENED for {DurationSeconds}s. Cause: {Message}");

    private static readonly Action<ILogger, Exception?> _logCircuitClosed =
        LoggerMessage.Define(LogLevel.Information, new EventId(5, "CircuitClosed"),
            "Redis circuit CLOSED. Connectivity restored.");

    private static readonly Action<ILogger, Exception?> _logCircuitHalfOpen =
        LoggerMessage.Define(LogLevel.Information, new EventId(6, "CircuitHalfOpen"),
            "Redis circuit HALF-OPEN. Probing...");

    private static readonly Action<ILogger, string, string, Exception?> _logBulkheadFull =
        LoggerMessage.Define<string, string>(LogLevel.Warning, new EventId(7, "BulkheadFull"),
            "Redis bulkhead full. {Operation} '{Key}' rejected — too many concurrent operations.");

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var fullKey = BuildKey(key);

        using var lease = _bulkhead.AttemptAcquire();
        if (!lease.IsAcquired)
        {
            _logBulkheadFull(logger, "GET", fullKey, null);
            return default;
        }

        try
        {
            return await _pipeline.ExecuteAsync(async ct =>
            {
                var value = await Db.StringGetAsync(fullKey);
                if (value.IsNullOrEmpty) return default;
                return JsonSerializer.Deserialize<T>((string)value!, JsonOpts);
            }, cancellationToken);
        }
        catch (BrokenCircuitException)
        {
            _logCircuitOpen(logger, "GET", fullKey, null);
            return default;
        }
        catch (Exception ex) when (ex is RedisException or TimeoutRejectedException)
        {
            _logOperationFailed(logger, "GET", fullKey, ex);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null, CancellationToken cancellationToken = default)
    {
        var fullKey = BuildKey(key);

        using var lease = _bulkhead.AttemptAcquire();
        if (!lease.IsAcquired)
        {
            _logBulkheadFull(logger, "SET", fullKey, null);
            return;
        }

        try
        {
            await _pipeline.ExecuteAsync(async ct =>
            {
                var serialized = JsonSerializer.Serialize(value, JsonOpts);
                var expiry = ttl ?? TimeSpan.FromSeconds(_options.DefaultTtlSeconds);
                // allkeys-lru eviction garantisi: bellek dolduğunda Redis eski key'leri
                // kaldırır ve bu SET her zaman başarılı olur. Catch yalnızca ağ hatalarını kapsar.
                await Db.StringSetAsync(fullKey, serialized, expiry);
            }, cancellationToken);
        }
        catch (BrokenCircuitException)
        {
            _logCircuitOpen(logger, "SET", fullKey, null);
        }
        catch (Exception ex) when (ex is RedisException or TimeoutRejectedException)
        {
            _logOperationFailed(logger, "SET", fullKey, ex);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var fullKey = BuildKey(key);

        using var lease = _bulkhead.AttemptAcquire();
        if (!lease.IsAcquired)
        {
            _logBulkheadFull(logger, "REMOVE", fullKey, null);
            return;
        }

        try
        {
            await _pipeline.ExecuteAsync(async ct =>
            {
                await Db.KeyDeleteAsync(fullKey);
            }, cancellationToken);
        }
        catch (BrokenCircuitException)
        {
            _logCircuitOpen(logger, "REMOVE", fullKey, null);
        }
        catch (Exception ex) when (ex is RedisException or TimeoutRejectedException)
        {
            _logOperationFailed(logger, "REMOVE", fullKey, ex);
        }
    }

    private string BuildKey(string key) => $"{_options.InstanceName}{key}";

    private static ConcurrencyLimiter CreateBulkhead(ResilienceOptions opts) =>
        new(new ConcurrencyLimiterOptions
        {
            PermitLimit = opts.MaxConcurrentOperations,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = opts.ConcurrentQueueLimit
        });

    private static ConnectionMultiplexer CreateConnection(CacheOptions opts)
    {
        var config = ConfigurationOptions.Parse(opts.ConnectionString);
        config.AbortOnConnectFail = false;
        config.ConnectRetry = 3;
        config.ReconnectRetryPolicy = new ExponentialRetry(5000);
        return ConnectionMultiplexer.Connect(config);
    }

    private static ResiliencePipeline BuildPipeline(ResilienceOptions opts, ILogger logger)
    {
        return new ResiliencePipelineBuilder()
            // 1. Timeout — her operasyon için bireysel zaman aşımı
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromMilliseconds(opts.OperationTimeoutMs)
            })
            // 2. Retry — geçici hatalarda üstel geri çekilme ile yeniden dene
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = opts.RetryCount - 1,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromMilliseconds(opts.RetryBaseDelayMs),
                ShouldHandle = new PredicateBuilder()
                    .Handle<RedisException>()
                    .Handle<TimeoutRejectedException>(),
                OnRetry = args =>
                {
                    _logRetry(logger,
                        args.AttemptNumber + 1,
                        opts.RetryCount - 1,
                        args.RetryDelay.TotalMilliseconds,
                        args.Outcome.Exception?.Message,
                        null);
                    return ValueTask.CompletedTask;
                }
            })
            // 3. Circuit Breaker — ardışık başarısızlıkta devre açılır, Redis'i korur
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = opts.CircuitBreakerFailureThreshold,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(opts.CircuitBreakerBreakDurationSeconds),
                ShouldHandle = new PredicateBuilder()
                    .Handle<RedisException>()
                    .Handle<TimeoutRejectedException>(),
                OnOpened = args =>
                {
                    _logCircuitOpened(logger,
                        opts.CircuitBreakerBreakDurationSeconds,
                        args.Outcome.Exception?.Message,
                        args.Outcome.Exception);
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    _logCircuitClosed(logger, null);
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = _ =>
                {
                    _logCircuitHalfOpen(logger, null);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    public async ValueTask DisposeAsync()
    {
        _bulkhead.Dispose();
        await _connection.CloseAsync();
        _connection.Dispose();
    }
}
