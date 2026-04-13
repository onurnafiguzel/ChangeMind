namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Coaches.Commands;
using ChangeMind.Application.UseCases.Coaches.Queries;

[ApiController]
[Route("api/coaches")]
public class CoachesController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Get all coaches with optional pagination and active filter
    /// </summary>
    [Authorize(Roles = "Admin,Coach")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<CoachDto>>> GetCoaches(
        [FromQuery] bool? isActiveOnly = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCoachesQuery
        {
            IsActiveOnly = isActiveOnly,
            Page = page,
            PageSize = pageSize
        };

        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get coach by ID
    /// </summary>
    [Authorize(Roles = "Admin,Coach")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CoachDto>> GetCoachById(Guid id, CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForCoach(id))
            return Forbid();

        var query = new GetCoachByIdQuery { CoachId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new coach (admin only)
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCoach(
        [FromBody] CreateCoachCommand command,
        CancellationToken cancellationToken = default)
    {
        var coachId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCoachById), new { id = coachId }, coachId);
    }

    /// <summary>
    /// Update coach profile
    /// </summary>
    [Authorize(Roles = "Admin,Coach")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCoach(
        Guid id,
        [FromBody] UpdateCoachCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForCoach(id))
            return Forbid();

        var command = baseRequest with { CoachId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete coach (sets IsActive = false)
    /// </summary>
    [Authorize(Roles = "Admin,Coach")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCoach(Guid id, CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForCoach(id))
            return Forbid();

        var command = new DeleteCoachCommand { CoachId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Change coach password
    /// </summary>
    [Authorize(Roles = "Admin,Coach")]
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(
        Guid id,
        [FromBody] ChangeCoachPasswordCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForCoach(id))
            return Forbid();

        var command = baseRequest with { CoachId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

}
