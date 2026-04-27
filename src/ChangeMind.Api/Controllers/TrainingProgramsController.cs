namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("~/api/users/{userId:guid}/active-program")]
    public async Task<ActionResult<ActiveProgramDetailDto>> GetUserActiveProgram(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserActiveProgramQuery { UserId = userId };
        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Update the daily exercise schedule of a training program. Increments version number.
    /// </summary>
    [HttpPut("{id:guid}/daily-program")]
    public async Task<IActionResult> UpdateDailyProgram(
        Guid id,
        [FromBody] UpdateDailyProgramCommand command,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(command with { ProgramId = id }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Activate a training program.
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new ActivateTrainingProgramCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deactivate a training program.
    /// </summary>
    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeactivateTrainingProgramCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Mark a training program as completed.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new CompleteTrainingProgramCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Update progress (completed weeks) for a training program.
    /// </summary>
    [HttpPut("{id:guid}/progress")]
    public async Task<IActionResult> UpdateProgress(
        Guid id,
        [FromBody] UpdateProgressRequest body,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdateProgressCommand(id, body.CompletedWeeks), cancellationToken);
        return NoContent();
    }
}

public record UpdateProgressRequest(int CompletedWeeks);
