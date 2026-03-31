namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class CoachRepository(ChangeMindDbContext context) : ICoachRepository
{
    public async Task<Coach?> GetByIdAsync(Guid id)
    {
        return await context.Coaches.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Coach?> GetByEmailAsync(string email)
    {
        return await context.Coaches.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email);
    }

    public IQueryable<Coach> GetAll(bool? isActive = null)
    {
        var query = context.Coaches.AsNoTracking().AsQueryable();

        if (isActive.HasValue)
            query = query.Where(c => c.IsActive == isActive.Value);

        return query;
    }

    public async Task AddAsync(Coach coach)
    {
        context.Coaches.Add(coach);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Coach coach)
    {
        context.Coaches.Update(coach);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await context.Coaches.AnyAsync(c => c.Email == email);
    }
}
