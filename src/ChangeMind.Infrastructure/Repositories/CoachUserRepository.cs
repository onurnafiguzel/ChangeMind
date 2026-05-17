namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Infrastructure.Data;

public class CoachUserRepository(ChangeMindDbContext context) : ICoachUserRepository
{
    public async Task<bool> IsActiveAssignmentAsync(Guid coachId, Guid userId)
    {
        return await context.CoachUsers
            .AsNoTracking()
            .AnyAsync(cu => cu.CoachId == coachId && cu.UserId == userId && cu.IsActive);
    }
}
