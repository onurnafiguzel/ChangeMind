namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record CreateSelfTrainingProgramCommand(
    Guid UserId,
    string Name,
    string? Description = null,
    int DurationWeeks = 12,
    DifficultyLevel? Difficulty = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Dictionary<string, List<ProgramExerciseInput>>? ExercisesByDay = null) : IRequest<Guid>;
