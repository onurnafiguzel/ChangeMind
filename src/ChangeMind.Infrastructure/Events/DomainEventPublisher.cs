namespace ChangeMind.Infrastructure.Events;

using ChangeMind.Application.Events;
using ChangeMind.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

public sealed class DomainEventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod(nameof(IEventHandler<IDomainEvent>.HandleAsync));
            if (method is not null)
                await (Task)method.Invoke(handler, [@event, cancellationToken])!;
        }
    }
}
