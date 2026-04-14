namespace ChangeMind.Application.UseCases.Exercises.Commands;

using MediatR;

public record UpdateExerciseCommand(
    Guid ExerciseId,
    string Name,
    string MuscleGroup,
    string DifficultyLevel,
    string? Description = null,
    string? VideoUrl = null) : IRequest;
