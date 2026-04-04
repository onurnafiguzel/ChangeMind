namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IWaitingUserRepository
{
    Task<WaitingUser?> GetByUserIdAsync(Guid userId);
    IQueryable<WaitingUser> GetWaitingForAssignment();
    Task AddAsync(WaitingUser waitingUser);
    Task UpdateAsync(WaitingUser waitingUser);
}
