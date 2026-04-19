namespace ChangeMind.Application.UseCases.Payments.Commands;

using MediatR;

public record ProcessPaymentCommand(
    Guid UserId,
    Guid PackageId,
    decimal Amount,
    string? Description = null,
    Guid? IdempotencyKey = null) : IRequest<PaymentProcessResponse>;

public class PaymentProcessResponse
{
    public bool Success { get; set; }
    public Guid PaymentId { get; set; }
    public string? Message { get; set; }
}
