namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class TrainingProgramRepository(ChangeMindDbContext context) : ITrainingProgramRepository
{

    public async Task AddAsync(TrainingProgram program)
    {
        await context.TrainingPrograms.AddAsync(program);
    }

    public Task UpdateAsync(TrainingProgram program)
    {
        context.TrainingPrograms.Update(program);
        return Task.CompletedTask;
    }

    public async Task<TrainingProgram?> GetByIdAsync(Guid id)
    {
        return await context.TrainingPrograms
            .Include(p => p.CreatedBy)
            .Include(p => p.AssignedTo).ThenInclude(u => u.Profile)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<TrainingProgram?> GetActiveByUserIdAsync(Guid userId)
    {
        var today = DateTime.UtcNow.Date;
        return await context.TrainingPrograms
            .Include(p => p.CreatedBy)
            .Include(p => p.AssignedTo).ThenInclude(u => u.Profile)
            .FirstOrDefaultAsync(p => p.UserId == userId
                                    && p.IsActive
                                    && (p.StartDate == null || p.StartDate.Value.Date <= today)
                                    && (p.EndDate == null || p.EndDate.Value.Date > today));
    }

    public async Task<IReadOnlyList<TrainingProgram>> GetActiveByCoachIdAsync(Guid coachId)
    {
        return await context.TrainingPrograms
            .Include(p => p.AssignedTo).ThenInclude(u => u.Profile)
            .AsNoTracking()
            .Where(p => p.CoachId == coachId && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
