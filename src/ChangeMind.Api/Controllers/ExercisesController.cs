namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Exercises.Commands;
using ChangeMind.Application.UseCases.Exercises.Queries;
using ChangeMind.Domain.Enums;

[ApiController]
[Route("api/exercises")]
public class ExercisesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all exercises with optional filters, search, sorting, and pagination.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ExerciseDto>>> GetExercises(
        [FromQuery] string? muscleGroup = null,
        [FromQuery] string? difficultyLevel = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool? isActiveOnly = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExercisesQuery
        {
            MuscleGroup     = muscleGroup is not null
                ? Enum.TryParse<MuscleGroup>(muscleGroup, ignoreCase: true, out var mg) ? mg : null
                : null,
            DifficultyLevel = difficultyLevel is not null
                ? Enum.TryParse<DifficultyLevel>(difficultyLevel, ignoreCase: true, out var dl) ? dl : null
                : null,
            Search      = search,
            SortBy      = sortBy,
            IsActiveOnly = isActiveOnly,
            Page        = page,
            PageSize    = pageSize
        };

        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get exercise by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> GetExerciseById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExerciseByIdQuery { ExerciseId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new exercise (Admin only).
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateExercise(
        [FromBody] CreateExerciseCommand command,
        CancellationToken cancellationToken = default)
    {
        var exerciseId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetExerciseById), new { id = exerciseId }, exerciseId);
    }

    /// <summary>
    /// Update an exercise (Admin only).
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateExercise(
        Guid id,
        [FromBody] UpdateExerciseCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        var command = baseRequest with { ExerciseId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete an exercise — sets IsActive = false (Admin only).
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExercise(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteExerciseCommand(id), cancellationToken);
        return NoContent();
    }
}
