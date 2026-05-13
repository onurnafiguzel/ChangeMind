namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Foods.Commands;
using ChangeMind.Application.UseCases.Foods.Queries;

[ApiController]
[Route("api/foods")]
public class FoodsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// List all active foods. Each item carries `unit` to indicate grams vs piece based macros.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<FoodDto>>> ListFoods(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new ListFoodsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a food by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FoodDto>> GetFoodById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetFoodByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new food (Coach or Admin only).
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateFood(
        [FromBody] CreateFoodCommand command,
        CancellationToken cancellationToken = default)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFoodById), new { id }, id);
    }

    /// <summary>
    /// Update a food (Coach or Admin only).
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateFood(
        Guid id,
        [FromBody] UpdateFoodCommand body,
        CancellationToken cancellationToken = default)
    {
        var command = body with { FoodId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete a food — sets IsActive = false (Coach or Admin only).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFood(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteFoodCommand(id), cancellationToken);
        return NoContent();
    }
}
