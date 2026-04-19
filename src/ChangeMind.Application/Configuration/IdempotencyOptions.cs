namespace ChangeMind.Application.Configuration;

public sealed class IdempotencyOptions
{
    /// <summary>In-flight distributed lock TTL. Must cover the worst-case handler duration.</summary>
    public int LockTtlMs { get; init; } = 5000;

    /// <summary>How long the cached response is retained for duplicate detection.</summary>
    public int ResponseTtlHours { get; init; } = 24;
}
