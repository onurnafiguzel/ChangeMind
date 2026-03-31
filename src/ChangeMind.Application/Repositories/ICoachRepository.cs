namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface ICoachRepository
{
    Task<Coach?> GetByIdAsync(Guid id);
    Task<Coach?> GetByEmailAsync(string email);
    IQueryable<Coach> GetAll(bool? isActive = null);
    Task AddAsync(Coach coach);
    Task UpdateAsync(Coach coach);
    Task<bool> ExistsAsync(string email);
}
