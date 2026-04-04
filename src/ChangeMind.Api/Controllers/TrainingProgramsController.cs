namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.UseCases.TrainingPrograms.Commands;

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
}
