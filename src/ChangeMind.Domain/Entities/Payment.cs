namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public sealed class Payment
{
    // EF Core constructor — object creation only through factory method
    private Payment() { }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public Guid PackageId { get; private set; }

    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public string? TransactionId { get; private set; }
    public string? Description { get; private set; }
    public string? IdempotencyKey { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Navigation Properties
    public User User { get; private set; } = null!;
    public Package Package { get; private set; } = null!;

    public static Payment Create(
        Guid userId,
        Guid packageId,
        decimal amount,
        string? description = null,
        Guid? idempotencyKey = null)
    {
        return new Payment
        {
            Id             = Guid.NewGuid(),
            UserId         = userId,
            PackageId      = packageId,
            Amount         = amount,
            Status         = PaymentStatus.Pending,
            Description    = description,
            IdempotencyKey = idempotencyKey?.ToString("D"),
            CreatedAt      = DateTime.UtcNow,
            CompletedAt    = null,
            TransactionId  = null
        };
    }

    public void MarkAsCompleted(string transactionId)
    {
        Status        = PaymentStatus.Completed;
        TransactionId = transactionId;
        CompletedAt   = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
    }

    public void MarkAsRefunded()
    {
        Status = PaymentStatus.Refunded;
    }
}
