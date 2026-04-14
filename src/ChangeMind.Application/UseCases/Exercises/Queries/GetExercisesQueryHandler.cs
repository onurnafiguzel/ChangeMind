namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetExercisesQueryHandler(IExerciseRepository exerciseRepository)
    : IRequestHandler<GetExercisesQuery, PagedResult<ExerciseDto>>
{
    public async Task<PagedResult<ExerciseDto>> Handle(
        GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var query = exerciseRepository.GetAll(
            muscleGroup:    request.MuscleGroup,
            difficultyLevel: request.DifficultyLevel,
            search:         request.Search,
            activeOnly:     request.IsActiveOnly ?? true);

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "musclegroup" => query.OrderBy(e => e.MuscleGroup).ThenBy(e => e.Name),
            "difficulty"  => query.OrderBy(e => e.DifficultyLevel).ThenBy(e => e.Name),
            _             => query.OrderBy(e => e.Name)
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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
            .ToListAsync(cancellationToken);

        return new PagedResult<ExerciseDto>
        {
            Data       = items,
            Total      = total,
            Page       = request.Page,
            PageSize   = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)total / request.PageSize)
        };
    }
}
