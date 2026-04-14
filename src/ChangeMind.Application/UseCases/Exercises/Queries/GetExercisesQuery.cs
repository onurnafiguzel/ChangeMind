namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Enums;
using MediatR;

public class GetExercisesQuery : IRequest<PagedResult<ExerciseDto>>
{
    public MuscleGroup? MuscleGroup { get; set; }
    public DifficultyLevel? DifficultyLevel { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }       // "name" | "muscleGroup" | "difficulty"
    public bool? IsActiveOnly { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
