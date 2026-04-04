namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Payment
{
    // Identifier
    public Guid Id { get; private set; }

    // Foreign Keys
    public Guid UserId { get; private set; }
    public Guid PackageId { get; private set; }

    // Primitive Properties
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public string? TransactionId { get; private set; }
    public string? Description { get; private set; }

    // DateTime Properties
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Navigation Properties
    public User User { get; private set; }
    public Package Package { get; private set; }

    // EF Constructor
    private protected Payment() { }

    /// <summary>
    /// Factory method to create a new payment
    /// </summary>
    public static Payment Create(
        Guid userId,
        Guid packageId,
        decimal amount,
        string? description = null)
    {
        return new Payment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PackageId = packageId,
            Amount = amount,
            Status = PaymentStatus.Pending,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = null,
            TransactionId = null
        };
    }

    /// <summary>
    /// Mark payment as completed
    /// </summary>
    public void MarkAsCompleted(string transactionId)
    {
        Status = PaymentStatus.Completed;
        TransactionId = transactionId;
        CompletedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark payment as failed
    /// </summary>
    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
    }

    /// <summary>
    /// Mark payment as refunded
    /// </summary>
    public void MarkAsRefunded()
    {
        Status = PaymentStatus.Refunded;
    }
}
