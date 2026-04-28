namespace ChangeMind.Domain.Events;

public sealed class UserDeactivatedEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; } = userId;
}
