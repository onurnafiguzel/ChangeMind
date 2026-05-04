namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class GetExerciseByIdQueryHandler(
    IExerciseRepository exerciseRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions)
    : IRequestHandler<GetExerciseByIdQuery, ExerciseDto>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<ExerciseDto> Handle(
        GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.Exercise(request.ExerciseId);

        var cached = await cache.GetAsync<ExerciseDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var exercise = await exerciseRepository
            .GetById(request.ExerciseId)
            .Select(e => new ExerciseDto
            {
                Id              = e.Id,
                Name            = e.Name,
                MuscleGroup     = e.MuscleGroup.ToString(),
                DifficultyLevel = e.DifficultyLevel.HasValue ? e.DifficultyLevel.Value.ToString() : null,
                Description     = e.Description,
                VideoUrl        = e.VideoUrl,
                IsActive        = e.IsActive,
                CreatedAt       = e.CreatedAt,
                UpdatedAt       = e.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Exercise with ID '{request.ExerciseId}' not found.");

        await cache.SetAsync(key, exercise, TimeSpan.FromSeconds(_cacheOptions.ExerciseTtlSeconds), cancellationToken);

        return exercise;
    }
}
