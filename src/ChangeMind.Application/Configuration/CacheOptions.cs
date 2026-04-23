namespace ChangeMind.Application.Configuration;

public sealed class CacheOptions
{
    public string ConnectionString { get; init; } = "localhost:6379";
    public int DefaultTtlSeconds { get; init; } = 300;
    public string InstanceName { get; init; } = "changemind:";
    public ResilienceOptions Resilience { get; init; } = new();
}

public sealed class ResilienceOptions
{
    /// <summary>Max attempt count including the first try.</summary>
    public int RetryCount { get; init; } = 3;

    /// <summary>Base delay between retries (exponential back-off is applied).</summary>
    public int RetryBaseDelayMs { get; init; } = 200;

    /// <summary>How many consecutive failures open the circuit.</summary>
    public int CircuitBreakerFailureThreshold { get; init; } = 5;

    /// <summary>How long the circuit stays open before allowing a probe attempt.</summary>
    public int CircuitBreakerBreakDurationSeconds { get; init; } = 30;

    /// <summary>Per-operation timeout (independent of the request-level timeout).</summary>
    public int OperationTimeoutMs { get; init; } = 1000;

    /// <summary>Maximum concurrent Redis operations. Excess requests are rejected immediately.</summary>
    public int MaxConcurrentOperations { get; init; } = 50;

    /// <summary>Queue depth when bulkhead is full. 0 = reject immediately (no queuing).</summary>
    public int ConcurrentQueueLimit { get; init; } = 0;
}
