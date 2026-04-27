namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;

public record UpdateDailyProgramCommand(
    Guid ProgramId,
    Dictionary<string, List<ProgramExerciseInput>> ExercisesByDay) : IRequest;
