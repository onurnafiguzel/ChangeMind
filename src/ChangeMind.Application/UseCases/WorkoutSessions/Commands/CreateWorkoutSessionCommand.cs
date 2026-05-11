namespace ChangeMind.Application.UseCases.WorkoutSessions.Commands;

using MediatR;

public record CreateWorkoutSessionCommand(
    Guid UserId,
    Guid? TrainingProgramId,
    string DayKey,
    DateOnly RecordDate,
    List<WorkoutExerciseInput> Exercises) : IRequest<Guid>;

public record WorkoutExerciseInput(Guid ExerciseId, List<WorkoutSetInput> Sets);

public record WorkoutSetInput(int SetNumber, decimal Weight, int Reps);
