namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface INutritionPlanRepository
{
    Task AddAsync(NutritionPlan plan);
    Task UpdateAsync(NutritionPlan plan);
    Task<NutritionPlan?> GetByIdAsync(Guid id);
    Task<NutritionPlan?> GetLatestActiveByUserIdAsync(Guid userId);
    Task<IReadOnlyList<NutritionPlan>> GetAllByUserIdAsync(Guid userId);
    Task<IReadOnlyList<NutritionPlan>> GetActiveByUserIdAsync(Guid userId);
}
