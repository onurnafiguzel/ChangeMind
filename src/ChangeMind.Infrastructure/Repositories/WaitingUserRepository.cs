namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class WaitingUserRepository(ChangeMindDbContext context) : IWaitingUserRepository
{

    public async Task<WaitingUser?> GetByUserIdAsync(Guid userId)
    {
        return await context.WaitingUsers.FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public IQueryable<WaitingUser> GetWaitingForAssignment()
    {
        return context.WaitingUsers
            .Include(w => w.User)
            .AsNoTracking()
            .Where(w => w.IsWaitingForAssignment)
            .AsQueryable();
    }

    public async Task AddAsync(WaitingUser waitingUser)
    {
        await context.WaitingUsers.AddAsync(waitingUser);
    }

    public Task UpdateAsync(WaitingUser waitingUser)
    {
        context.WaitingUsers.Update(waitingUser);
        return Task.CompletedTask;
    }
}
