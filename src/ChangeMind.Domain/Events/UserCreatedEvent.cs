namespace ChangeMind.Domain.Events;

public sealed class UserCreatedEvent(Guid userId, string email) : DomainEvent
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
}
