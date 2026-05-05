namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class GetExercisesQueryHandler(
    IExerciseRepository exerciseRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions) : IRequestHandler<GetExercisesQuery, PagedResult<ExerciseDto>>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<PagedResult<ExerciseDto>> Handle(
        GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.ExerciseList(
            request.MuscleGroup?.ToString(), request.DifficultyLevel?.ToString(),
            request.Search, request.SortBy, request.IsActiveOnly,
            request.Page, request.PageSize);

        var cached = await cache.GetAsync<PagedResult<ExerciseDto>>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var query = exerciseRepository.GetAll(
            muscleGroup:    request.MuscleGroup,
            difficultyLevel: request.DifficultyLevel,
            search:         request.Search,
            activeOnly:     request.IsActiveOnly ?? true);

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "musclegroup" => query.OrderBy(e => e.MuscleGroup).ThenBy(e => e.MovementName),
            "difficulty"  => query.OrderBy(e => e.DifficultyLevel).ThenBy(e => e.MovementName),
            _             => query.OrderBy(e => e.MovementName)
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ExerciseDto
            {
                Id              = e.Id,
                Name            = e.MovementName,
                MuscleGroup     = e.MuscleGroup.ToString(),
                DifficultyLevel = e.DifficultyLevel.HasValue ? e.DifficultyLevel.Value.ToString() : null,
                Description     = e.Description,
                VideoUrl        = e.VideoLink,
                IsActive        = e.IsActive,
                CreatedAt       = e.CreatedAt,
                UpdatedAt       = e.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        var result = new PagedResult<ExerciseDto>
        {
            Data       = items,
            Total      = total,
            Page       = request.Page,
            PageSize   = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)total / request.PageSize)
        };

        await cache.SetAsync(key, result, TimeSpan.FromSeconds(_cacheOptions.ExerciseTtlSeconds), cancellationToken);

        return result;
    }
}
