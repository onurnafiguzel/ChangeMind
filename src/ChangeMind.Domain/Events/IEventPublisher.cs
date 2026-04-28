namespace ChangeMind.Domain.Events;

public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
}
