namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetExerciseByIdQueryHandler(IExerciseRepository exerciseRepository)
    : IRequestHandler<GetExerciseByIdQuery, ExerciseDto>
{
    public async Task<ExerciseDto> Handle(
        GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
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

        return exercise;
    }
}
