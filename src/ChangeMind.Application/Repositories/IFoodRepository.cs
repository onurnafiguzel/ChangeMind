namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IFoodRepository
{
    Task<IReadOnlyList<Food>> ListActiveAsync();
    Task<Food?> GetByIdAsync(Guid id);
    Task<bool> AllExistAsync(IEnumerable<Guid> ids);
    Task AddAsync(Food food);
    Task UpdateAsync(Food food);
}
