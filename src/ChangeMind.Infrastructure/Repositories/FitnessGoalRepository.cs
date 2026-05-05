namespace ChangeMind.Infrastructure.Repositories;

using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class FitnessGoalRepository(ChangeMindDbContext context) : IFitnessGoalRepository
{
    public async Task<FitnessGoalItem?> GetByIdAsync(Guid id)
        => await context.FitnessGoals.FirstOrDefaultAsync(f => f.Id == id);

    public IQueryable<FitnessGoalItem> GetAll()
        => context.FitnessGoals.AsNoTracking().AsQueryable();

    public async Task AddAsync(FitnessGoalItem item)
        => await context.FitnessGoals.AddAsync(item);

    public Task UpdateAsync(FitnessGoalItem item)
    {
        context.FitnessGoals.Update(item);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name)
        => await context.FitnessGoals.AnyAsync(f => f.Name == name);
}
