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

    public async Task<TrainingProgram?> GetActiveByUserIdAsync(Guid userId)
    {
        var today = DateTime.UtcNow.Date;
        return await context.TrainingPrograms
            .Include(p => p.CreatedBy)
            .FirstOrDefaultAsync(p => p.UserId == userId
                                    && p.IsActive
                                    && (p.StartDate == null || p.StartDate.Value.Date <= today)
                                    && (p.EndDate == null || p.EndDate.Value.Date > today));
    }
}
