namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.TrainingPrograms.Commands;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;
using System.Security.Claims;

[ApiController]
[Route("api/training-programs")]
public class TrainingProgramsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Create a new training program and assign to a user
    /// When a program is created, the user is marked as no longer waiting for assignment
    /// </summary>
    [Authorize(Roles = "Coach")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTrainingProgram(
        [FromBody] CreateTrainingProgramCommand command,
        CancellationToken cancellationToken = default)
    {
        var programId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateTrainingProgram), new { id = programId }, programId);
    }

    /// <summary>
    /// Get user's active training program with daily exercises
    /// User can have at most 1 active program at a time
    /// </summary>
    [Authorize(Roles = "User,Admin")]
    [HttpGet("~/api/users/{userId:guid}/active-program")]
    public async Task<ActionResult<ActiveProgramDetailDto>> GetUserActiveProgram(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(userId))
            return Forbid();

        var query = new GetUserActiveProgramQuery
        {
            UserId = userId
        };

        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    private bool IsAuthorizedForUser(Guid userId)
    {
        // Get userId from JWT token claims
        var tokenUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        // Allow if user ID matches OR user is Admin
        if (Guid.TryParse(tokenUserIdClaim, out var tokenUserId))
        {
            return tokenUserId == userId || userRoleClaim == "Admin";
        }

        return false;
    }
}
