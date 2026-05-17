namespace ChangeMind.Application.UseCases.WorkoutSessions.Commands;

using System.Text.Json;
using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;

public class CreateWorkoutSessionCommandHandler(
    IWorkoutSessionRepository workoutSessionRepository,
    ITrainingProgramRepository trainingProgramRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateWorkoutSessionCommand, Guid>
{
    public async Task<Guid> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        if (request.TrainingProgramId is { } programId)
        {
            var program = await trainingProgramRepository.GetByIdAsync(programId);
            if (program is null || program.UserId != request.UserId)
                throw new ValidationException("TrainingProgramId user ile uyuşmuyor.");
        }

        var payload = new
        {
            exercises = request.Exercises.Select(e => new
            {
                exerciseId = e.ExerciseId,
                sets = e.Sets.Select(s => new
                {
                    setNumber = s.SetNumber,
                    weight = s.Weight,
                    reps = s.Reps
                })
            })
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var session = WorkoutSession.Create(
            request.UserId,
            request.TrainingProgramId,
            request.DayKey,
            request.RecordDate,
            json);

        await workoutSessionRepository.AddAsync(session);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}
