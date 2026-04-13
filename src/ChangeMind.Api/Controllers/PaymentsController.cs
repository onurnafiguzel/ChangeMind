namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Payments.Commands;

[ApiController]
[Route("api/payments")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Process a payment for a package purchase (creates a payment resource).
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PaymentProcessResponse>> ProcessPayment(
        [FromBody] ProcessPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
