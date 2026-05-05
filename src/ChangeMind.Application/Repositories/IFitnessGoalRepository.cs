namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IFitnessGoalRepository
{
    Task<FitnessGoalItem?> GetByIdAsync(Guid id);
    IQueryable<FitnessGoalItem> GetAll();
    Task AddAsync(FitnessGoalItem item);
    Task UpdateAsync(FitnessGoalItem item);
    Task<bool> ExistsAsync(string name);
}
