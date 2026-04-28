namespace ChangeMind.Application.Events;

using ChangeMind.Domain.Events;

public interface IEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
