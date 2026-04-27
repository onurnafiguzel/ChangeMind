namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface ITrainingProgramRepository
{
    Task AddAsync(TrainingProgram program);
    Task UpdateAsync(TrainingProgram program);
    Task<TrainingProgram?> GetByIdAsync(Guid id);
    Task<TrainingProgram?> GetActiveByUserIdAsync(Guid userId);
}
