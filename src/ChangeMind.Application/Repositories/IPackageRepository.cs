namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IPackageRepository
{
    Task<Package?> GetByIdAsync(Guid id);
    IQueryable<Package> GetById(Guid id);
    IQueryable<Package> GetAll(bool? isActive = null);
    Task AddAsync(Package package);
    Task UpdateAsync(Package package);
    Task DeleteAsync(Package package);
    Task<bool> ExistsAsync(string name);
}
