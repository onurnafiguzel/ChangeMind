namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class PackageRepository(ChangeMindDbContext context) : IPackageRepository
{
    public async Task<Package?> GetByIdAsync(Guid id)
    {
        return await context.Packages.FirstOrDefaultAsync(p => p.Id == id);
    }

    public IQueryable<Package> GetAll(bool? isActive = null)
    {
        var query = context.Packages.AsNoTracking().AsQueryable();

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        return query;
    }

    public async Task AddAsync(Package package)
    {
        await context.Packages.AddAsync(package);
    }

    public Task UpdateAsync(Package package)
    {
        context.Packages.Update(package);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Package package)
    {
        context.Packages.Remove(package);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await context.Packages.AnyAsync(p => p.Name == name);
    }
}
