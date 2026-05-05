namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.FitnessGoals.Commands;
using ChangeMind.Application.UseCases.FitnessGoals.Queries;

[ApiController]
[Route("api/fitness-goals")]
public class FitnessGoalsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// List all fitness goals. No authentication required.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<FitnessGoalDto>>> GetFitnessGoals(
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetFitnessGoalsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new fitness goal (Coach or Admin only).
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateFitnessGoal(
        [FromBody] CreateFitnessGoalCommand command,
        CancellationToken cancellationToken = default)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFitnessGoals), new { }, id);
    }

    /// <summary>
    /// Update a fitness goal (Coach or Admin only).
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateFitnessGoal(
        Guid id,
        [FromBody] UpdateFitnessGoalCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        var command = baseRequest with { Id = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete a fitness goal — sets IsActive = false (Coach or Admin only).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFitnessGoal(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteFitnessGoalCommand(id), cancellationToken);
        return NoContent();
    }
}
