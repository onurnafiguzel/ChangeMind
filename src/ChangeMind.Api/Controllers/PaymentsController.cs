namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.UseCases.Payments.Commands;

[ApiController]
[Route("api/payments")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Process a payment for a package purchase (creates a payment resource).
    /// </summary>
    [HttpPost]
    [RequestTimeout("external")]
    public async Task<ActionResult<PaymentProcessResponse>> ProcessPayment(
        [FromBody] ProcessPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
