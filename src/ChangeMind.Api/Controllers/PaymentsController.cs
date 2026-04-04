namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Payments.Commands;
using ChangeMind.Application.UseCases.Users.Queries;

[ApiController]
[Route("api/payments")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Process a payment for a package purchase
    /// </summary>
    [Authorize]
    [HttpPost("process")]
    public async Task<ActionResult<PaymentProcessResponse>> ProcessPayment(
        [FromBody] ProcessPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get list of users waiting for coach assignment.
    /// These are users with completed payments and IsWaitingForAssignment = true.
    /// Coaches only.
    /// </summary>
    [Authorize(Roles = "Coach,Admin")]
    [HttpGet("waiting-users")]
    public async Task<ActionResult<List<UserAssignmentDto>>> GetWaitingUsers(
        CancellationToken cancellationToken = default)
    {
        var query = new GetWaitingUsersQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
