namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Coaches.Commands;
using ChangeMind.Application.UseCases.Coaches.Queries;

[ApiController]
[Route("api/coaches")]
public class CoachesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all coaches with optional pagination and active filter
    /// </summary>
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
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CoachDto>> GetCoachById(Guid id, CancellationToken cancellationToken = default)
    {

        var query = new GetCoachByIdQuery { CoachId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new coach (admin only)
    /// </summary>
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
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCoach(
        Guid id,
        [FromBody] UpdateCoachCommand baseRequest,
        CancellationToken cancellationToken = default)
    {

        var command = baseRequest with { CoachId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete coach (sets IsActive = false)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCoach(Guid id, CancellationToken cancellationToken = default)
    {

        var command = new DeleteCoachCommand { CoachId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Change coach password
    /// </summary>
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(
        Guid id,
        [FromBody] ChangeCoachPasswordCommand baseRequest,
        CancellationToken cancellationToken = default)
    {

        var command = baseRequest with { CoachId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

}
