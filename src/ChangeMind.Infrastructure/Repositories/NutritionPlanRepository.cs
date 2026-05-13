namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class NutritionPlanRepository(ChangeMindDbContext context) : INutritionPlanRepository
{
    public async Task AddAsync(NutritionPlan plan) => await context.NutritionPlans.AddAsync(plan);

    public Task UpdateAsync(NutritionPlan plan)
    {
        context.NutritionPlans.Update(plan);
        return Task.CompletedTask;
    }

    public async Task<NutritionPlan?> GetByIdAsync(Guid id)
    {
        return await context.NutritionPlans
            .Include(np => np.Coach)
            .Include(np => np.User)
            .FirstOrDefaultAsync(np => np.Id == id);
    }

    public async Task<NutritionPlan?> GetLatestActiveByUserIdAsync(Guid userId)
    {
        return await context.NutritionPlans
            .Include(np => np.Coach)
            .Include(np => np.User)
            .Where(np => np.UserId == userId && np.IsActive)
            .OrderByDescending(np => np.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<NutritionPlan>> GetAllByUserIdAsync(Guid userId)
    {
        return await context.NutritionPlans
            .Include(np => np.Coach)
            .AsNoTracking()
            .Where(np => np.UserId == userId)
            .OrderByDescending(np => np.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<NutritionPlan>> GetActiveByUserIdAsync(Guid userId)
    {
        return await context.NutritionPlans
            .Where(np => np.UserId == userId && np.IsActive)
            .ToListAsync();
    }
}
