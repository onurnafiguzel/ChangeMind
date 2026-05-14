namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class ExportTrainingProgramQueryHandler(
    IMediator mediator,
    IWorkoutSessionRepository workoutSessionRepository,
    IExerciseRepository exerciseRepository)
    : IRequestHandler<ExportTrainingProgramQuery, ProgramExportResult?>
{
    public async Task<ProgramExportResult?> Handle(ExportTrainingProgramQuery request, CancellationToken cancellationToken)
    {
        var program = await mediator.Send(new GetTrainingProgramByIdQuery { ProgramId = request.ProgramId }, cancellationToken);
        if (program is null)
            return null;

        var sessionsRaw = await workoutSessionRepository.GetByUserAndProgramAsync(program.UserId, program.Id);
        var sessions = sessionsRaw
            .Select(s => new WorkoutSessionDto
            {
                Id                = s.Id,
                UserId            = s.UserId,
                TrainingProgramId = s.TrainingProgramId,
                DayKey            = s.DayKey,
                RecordDate        = s.RecordDate,
                CreatedAt         = s.CreatedAt,
                Exercises         = WorkoutSessionJson.DeserializeExercises(s.SessionDataJson)
            })
            .ToList();

        var exerciseIds = new HashSet<Guid>();
        if (program.DailyExercises is not null)
        {
            foreach (var list in program.DailyExercises.Values)
                foreach (var ex in list)
                    exerciseIds.Add(ex.ExerciseId);
        }
        foreach (var s in sessions)
            foreach (var ex in s.Exercises)
                exerciseIds.Add(ex.ExerciseId);

        var exerciseNames = new Dictionary<Guid, string>();
        foreach (var id in exerciseIds)
        {
            var ex = await exerciseRepository.GetByIdAsync(id);
            exerciseNames[id] = ex?.MovementName ?? id.ToString();
        }

        var bytes = TrainingProgramExcelBuilder.Build(program, sessions, exerciseNames);
        var safeName = SanitizeFileName(string.IsNullOrWhiteSpace(program.Name) ? "TrainingProgram" : program.Name);
        var fileName = $"{safeName}_{DateTime.UtcNow:yyyyMMdd}.xlsx";

        return new ProgramExportResult(fileName, bytes);
    }

    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        return cleaned.Replace(' ', '_');
    }
}
