namespace ChangeMind.Infrastructure.UnitOfWork;

using ChangeMind.Application.UnitOfWork;
using ChangeMind.Infrastructure.Data;

public class UnitOfWork(ChangeMindDbContext context) : IUnitOfWork
{
    /// <summary>
    /// Saves all pending changes to the database.
    /// Executes all pending Add, Update, Delete operations in a single transaction.
    /// </summary>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the context resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
