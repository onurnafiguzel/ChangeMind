namespace ChangeMind.Application.UseCases.Payments.Queries;

using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;
using MediatR;

public sealed class GetPaymentQueryHandler(IPaymentRepository paymentRepository)
    : IRequestHandler<GetPaymentQuery, PaymentDto>
{
    public async Task<PaymentDto> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(request.PaymentId)
            ?? throw new NotFoundException($"Payment with id '{request.PaymentId}' not found.");

        return new PaymentDto(
            payment.Id,
            payment.UserId,
            payment.PackageId,
            payment.Amount,
            payment.Status,
            payment.TransactionId,
            payment.Description,
            payment.CreatedAt,
            payment.CompletedAt);
    }
}
