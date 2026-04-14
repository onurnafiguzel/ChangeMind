namespace ChangeMind.Infrastructure.Repositories;

using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ExerciseRepository(ChangeMindDbContext context) : IExerciseRepository
{
    public IQueryable<Exercise> GetById(Guid id)
        => context.Exercises.AsNoTracking().Where(e => e.Id == id);

    public async Task<Exercise?> GetByIdAsync(Guid id)
        => await context.Exercises.FirstOrDefaultAsync(e => e.Id == id);

    public IQueryable<Exercise> GetAll(
        MuscleGroup? muscleGroup = null,
        DifficultyLevel? difficultyLevel = null,
        string? search = null,
        bool activeOnly = true)
    {
        var query = context.Exercises.AsNoTracking().AsQueryable();

        if (activeOnly)
            query = query.Where(e => e.IsActive);

        if (muscleGroup.HasValue)
            query = query.Where(e => e.MuscleGroup == muscleGroup.Value);

        if (difficultyLevel.HasValue)
            query = query.Where(e => e.DifficultyLevel == difficultyLevel.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));

        return query;
    }

    public async Task AddAsync(Exercise exercise)
        => await context.Exercises.AddAsync(exercise);

    public Task UpdateAsync(Exercise exercise)
    {
        context.Exercises.Update(exercise);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string name)
        => await context.Exercises.AnyAsync(e => e.Name == name && e.IsActive);
}
