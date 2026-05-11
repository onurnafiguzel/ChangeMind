namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class WorkoutSessionRepository(ChangeMindDbContext context) : IWorkoutSessionRepository
{
    public async Task AddAsync(WorkoutSession session)
    {
        await context.WorkoutSessions.AddAsync(session);
    }

    public async Task<IReadOnlyList<WorkoutSession>> GetByUserAndDayAsync(
        Guid userId, string dayKey, int count)
    {
        return await context.WorkoutSessions
            .AsNoTracking()
            .Where(s => s.UserId == userId && s.DayKey == dayKey)
            .OrderByDescending(s => s.RecordDate)
            .ThenByDescending(s => s.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
