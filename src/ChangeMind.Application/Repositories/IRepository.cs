namespace ChangeMind.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
}
