namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IWorkoutSessionRepository
{
    Task AddAsync(WorkoutSession session);
    Task<IReadOnlyList<WorkoutSession>> GetByUserAndDayAsync(Guid userId, string dayKey, int count);
}
