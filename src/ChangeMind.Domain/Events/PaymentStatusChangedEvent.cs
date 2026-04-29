namespace ChangeMind.Domain.Events;

using ChangeMind.Domain.Enums;

public sealed class PaymentStatusChangedEvent(
    Guid paymentId,
    Guid userId,
    PaymentStatus oldStatus,
    PaymentStatus newStatus,
    decimal amount) : DomainEvent
{
    public Guid PaymentId { get; } = paymentId;
    public Guid UserId { get; } = userId;
    public PaymentStatus OldStatus { get; } = oldStatus;
    public PaymentStatus NewStatus { get; } = newStatus;
    public decimal Amount { get; } = amount;
}
