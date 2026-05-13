namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class FoodRepository(ChangeMindDbContext context) : IFoodRepository
{
    public async Task<IReadOnlyList<Food>> ListActiveAsync()
    {
        return await context.Foods
            .AsNoTracking()
            .Where(f => f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<Food?> GetByIdAsync(Guid id)
    {
        return await context.Foods.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<bool> AllExistAsync(IEnumerable<Guid> ids)
    {
        var list = ids.Distinct().ToList();
        if (list.Count == 0) return true;
        var count = await context.Foods
            .AsNoTracking()
            .CountAsync(f => list.Contains(f.Id) && f.IsActive);
        return count == list.Count;
    }

    public async Task AddAsync(Food food) => await context.Foods.AddAsync(food);

    public Task UpdateAsync(Food food)
    {
        context.Foods.Update(food);
        return Task.CompletedTask;
    }
}
