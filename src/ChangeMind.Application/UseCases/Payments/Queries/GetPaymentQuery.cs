namespace ChangeMind.Application.UseCases.Payments.Queries;

using ChangeMind.Domain.Enums;
using MediatR;

public record GetPaymentQuery(Guid PaymentId) : IRequest<PaymentDto>;

public record PaymentDto(
    Guid Id,
    Guid UserId,
    Guid PackageId,
    decimal Amount,
    PaymentStatus Status,
    string? TransactionId,
    string? Description,
    DateTime CreatedAt,
    DateTime? CompletedAt);
