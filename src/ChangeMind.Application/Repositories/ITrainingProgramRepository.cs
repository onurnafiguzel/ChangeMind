namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface ITrainingProgramRepository
{
    Task AddAsync(TrainingProgram program);
    Task<TrainingProgram?> GetActiveByUserIdAsync(Guid userId);
}
