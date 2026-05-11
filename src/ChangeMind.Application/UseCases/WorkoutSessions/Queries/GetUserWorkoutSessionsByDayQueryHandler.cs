namespace ChangeMind.Application.UseCases.WorkoutSessions.Queries;

using System.Text.Json;
using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserWorkoutSessionsByDayQueryHandler(
    IWorkoutSessionRepository workoutSessionRepository)
    : IRequestHandler<GetUserWorkoutSessionsByDayQuery, List<WorkoutSessionDto>>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<WorkoutSessionDto>> Handle(
        GetUserWorkoutSessionsByDayQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Count < 1) request.Count = 4;
        if (request.Count > 50) request.Count = 50;

        var sessions = await workoutSessionRepository.GetByUserAndDayAsync(
            request.UserId, request.DayKey, request.Count);

        return sessions.Select(s => new WorkoutSessionDto
        {
            Id                = s.Id,
            UserId            = s.UserId,
            TrainingProgramId = s.TrainingProgramId,
            DayKey            = s.DayKey,
            RecordDate        = s.RecordDate,
            CreatedAt         = s.CreatedAt,
            Exercises         = DeserializeExercises(s.SessionDataJson)
        }).ToList();
    }

    private static List<WorkoutExerciseDto> DeserializeExercises(string sessionDataJson)
    {
        if (string.IsNullOrWhiteSpace(sessionDataJson))
            return new List<WorkoutExerciseDto>();

        try
        {
            var payload = JsonSerializer.Deserialize<SessionPayload>(sessionDataJson, JsonOptions);
            return payload?.Exercises ?? new List<WorkoutExerciseDto>();
        }
        catch
        {
            return new List<WorkoutExerciseDto>();
        }
    }

    private sealed class SessionPayload
    {
        public List<WorkoutExerciseDto> Exercises { get; set; } = new();
    }
}
