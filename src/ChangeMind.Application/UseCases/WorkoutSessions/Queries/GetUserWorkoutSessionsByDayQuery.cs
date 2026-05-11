namespace ChangeMind.Application.UseCases.WorkoutSessions.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetUserWorkoutSessionsByDayQuery : IRequest<List<WorkoutSessionDto>>
{
    public Guid UserId { get; set; }
    public string DayKey { get; set; } = string.Empty;
    public int Count { get; set; } = 4;
}
