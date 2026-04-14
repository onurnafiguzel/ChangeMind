namespace ChangeMind.Application.UseCases.Exercises.Commands;

using ChangeMind.Domain.Enums;
using MediatR;

public record CreateExerciseCommand(
    string Name,
    string MuscleGroup,
    string DifficultyLevel,
    string? Description = null,
    string? VideoUrl = null) : IRequest<Guid>;
