namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.TrainingPrograms.Commands;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;

[ApiController]
[Route("api/training-programs")]
public class TrainingProgramsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get a training program by ID.
    /// </summary>
    [Authorize(Roles = "Coach,Admin,User")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ActiveProgramDetailDto>> GetTrainingProgramById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTrainingProgramByIdQuery { ProgramId = id };
        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new training program and assign it to a user (Coach only).
    /// When a program is created, the user is marked as no longer waiting for assignment.
    /// </summary>
    [Authorize(Roles = "Coach")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTrainingProgram(
        [FromBody] CreateTrainingProgramCommand command,
        CancellationToken cancellationToken = default)
    {
        var programId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTrainingProgramById), new { id = programId }, programId);
    }

    /// <summary>
    /// Get a user's active training program with daily exercises.
    /// A user can have at most one active program at a time.
    /// </summary>
    [Authorize(Roles = "User,Admin")]
    [HttpGet("~/api/users/{userId:guid}/active-program")]
    public async Task<ActionResult<ActiveProgramDetailDto>> GetUserActiveProgram(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(userId))
            return Forbid();

        var query = new GetUserActiveProgramQuery { UserId = userId };
        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    private bool IsAuthorizedForUser(Guid userId)
    {
        var tokenUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (Guid.TryParse(tokenUserIdClaim, out var tokenUserId))
            return tokenUserId == userId || userRoleClaim == "Admin";

        return false;
    }
}
