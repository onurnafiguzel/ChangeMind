namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Events;
using ChangeMind.Domain.Exceptions;

public sealed class Payment : AggregateRoot
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

    /// <summary>Package access window start — set on MarkAsCompleted.</summary>
    public DateTime? PackageStartDate { get; private set; }

    /// <summary>Package access window end — set on MarkAsCompleted = Start + Package.DurationDays.</summary>
    public DateTime? PackageEndDate { get; private set; }

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

    public void MarkAsCompleted(string transactionId, int packageDurationDays)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidStateTransitionException(nameof(Payment), Status.ToString(), nameof(MarkAsCompleted));
        if (packageDurationDays <= 0)
            throw new ValidationException("Package duration must be positive.");

        var now           = DateTime.UtcNow;
        var old           = Status;
        Status            = PaymentStatus.Completed;
        TransactionId     = transactionId;
        CompletedAt       = now;
        PackageStartDate  = now;
        PackageEndDate    = now.AddDays(packageDurationDays);
        AddDomainEvent(new PaymentStatusChangedEvent(Id, UserId, old, Status, Amount));
    }

    public void MarkAsFailed()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidStateTransitionException(nameof(Payment), Status.ToString(), nameof(MarkAsFailed));

        var old = Status;
        Status = PaymentStatus.Failed;
        AddDomainEvent(new PaymentStatusChangedEvent(Id, UserId, old, Status, Amount));
    }

    public void MarkAsRefunded()
    {
        if (Status != PaymentStatus.Completed)
            throw new InvalidStateTransitionException(nameof(Payment), Status.ToString(), nameof(MarkAsRefunded));

        var old = Status;
        Status = PaymentStatus.Refunded;
        AddDomainEvent(new PaymentStatusChangedEvent(Id, UserId, old, Status, Amount));
    }
}
