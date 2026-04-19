namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Api.Filters;
using ChangeMind.Application.UseCases.Payments.Commands;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Process a payment for a package purchase (creates a payment resource).
    /// </summary>
    [HttpPost]
    [RequestTimeout("external")]
    [Idempotent]
    public async Task<ActionResult<PaymentProcessResponse>> ProcessPayment(
        [FromBody] ProcessPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        var idempotencyKey = HttpContext.Items["IdempotencyKey"] as Guid?;
        var result = await mediator.Send(command with { IdempotencyKey = idempotencyKey }, cancellationToken);
        return Ok(result);
    }
}
