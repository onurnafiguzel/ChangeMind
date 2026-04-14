namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;

public interface IExerciseRepository
{
    IQueryable<Exercise> GetById(Guid id);
    Task<Exercise?> GetByIdAsync(Guid id);
    IQueryable<Exercise> GetAll(
        MuscleGroup? muscleGroup = null,
        DifficultyLevel? difficultyLevel = null,
        string? search = null,
        bool activeOnly = true);
    Task AddAsync(Exercise exercise);
    Task UpdateAsync(Exercise exercise);
    Task<bool> ExistsAsync(string name);
}
