namespace ChangeMind.Application.Configuration;

public sealed class BulkheadOptions
{
    /// <summary>Maximum concurrent payment requests processed simultaneously.</summary>
    public int PaymentMaxConcurrent { get; init; } = 10;

    /// <summary>Queue depth when payment bulkhead is full. Queued requests wait for a slot.</summary>
    public int PaymentQueueLimit { get; init; } = 5;
}
