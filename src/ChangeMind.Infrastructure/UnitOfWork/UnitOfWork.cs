namespace ChangeMind.Infrastructure.UnitOfWork;

using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Events;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class UnitOfWork(ChangeMindDbContext context, IEventPublisher eventPublisher) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events before saving so we don't miss any cleared after save
        var aggregates = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var result = await context.SaveChangesAsync(cancellationToken);

        // Dispatch events after successful save
        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.DomainEvents)
                await eventPublisher.PublishAsync(domainEvent, cancellationToken);

            aggregate.ClearDomainEvents();
        }

        return result;
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
