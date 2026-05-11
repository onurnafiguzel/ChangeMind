namespace ChangeMind.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.WorkoutSessions.Commands;
using ChangeMind.Application.UseCases.WorkoutSessions.Queries;
using ChangeMind.Domain.Exceptions;

[ApiController]
[Route("api/workout-sessions")]
[Authorize]
public class WorkoutSessionsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Create a new workout session. The session is stored as a single row with
    /// the exercise/set/weight/reps data serialized into a JSON column.
    /// Users may only create sessions for themselves.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateWorkoutSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        EnsureSelf(command.UserId, "You can only log your own workout sessions.");

        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(
            nameof(GetByUserAndDay),
            new { userId = command.UserId, dayKey = command.DayKey },
            id);
    }

    /// <summary>
    /// Get the last N workout sessions for the given user + day key (e.g. "Day-3").
    /// Returns the JSON exercise/set data deserialized for easy consumption.
    /// Users may only query their own sessions.
    /// </summary>
    [HttpGet("~/api/users/{userId:guid}/workout-sessions")]
    public async Task<ActionResult<List<WorkoutSessionDto>>> GetByUserAndDay(
        Guid userId,
        [FromQuery] string dayKey,
        [FromQuery] int count = 4,
        CancellationToken cancellationToken = default)
    {
        EnsureSelf(userId, "You can only view your own workout sessions.");

        var query  = new GetUserWorkoutSessionsByDayQuery
        {
            UserId  = userId,
            DayKey  = dayKey,
            Count   = count
        };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    private void EnsureSelf(Guid pathOrBodyUserId, string forbiddenMessage)
    {
        var tokenUserId = User.FindFirstValue("sub");
        if (!Guid.TryParse(tokenUserId, out var tokenGuid) || tokenGuid != pathOrBodyUserId)
            throw new ForbiddenException(forbiddenMessage);
    }
}
