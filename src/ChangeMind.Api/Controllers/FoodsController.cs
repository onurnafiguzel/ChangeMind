namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Foods.Queries;

[ApiController]
[Route("api/foods")]
public class FoodsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// List all active foods (per 100g raw weight).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<FoodDto>>> ListFoods(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new ListFoodsQuery(), cancellationToken);
        return Ok(result);
    }
}
