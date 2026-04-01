namespace ChangeMind.Application.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Save all pending changes to the database as a single transaction.
    /// All repository operations (Add, Update, Delete) must be followed by SaveChangesAsync.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
