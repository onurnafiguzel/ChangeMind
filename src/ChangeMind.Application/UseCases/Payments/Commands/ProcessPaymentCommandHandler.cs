namespace ChangeMind.Application.UseCases.Payments.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;

public class ProcessPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IUserRepository userRepository,
    IPackageRepository packageRepository,
    IWaitingUserRepository waitingUserRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProcessPaymentCommand, PaymentProcessResponse>
{
    public async Task<PaymentProcessResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        var package = await packageRepository.GetByIdAsync(request.PackageId)
            ?? throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        var paymentAmount = request.Amount > 0 ? request.Amount : package.Price;

        var payment = Payment.Create(
            userId: request.UserId,
            packageId: request.PackageId,
            amount: paymentAmount,
            description: request.Description,
            idempotencyKey: request.IdempotencyKey);

        // Mock payment gateway — replace with real provider (Stripe, PayPal, etc.)
        payment.MarkAsCompleted(transactionId: Guid.NewGuid().ToString());

        await paymentRepository.AddAsync(payment);

        var existingWaitingUser = await waitingUserRepository.GetByUserIdAsync(request.UserId);
        if (existingWaitingUser == null)
        {
            var waitingUser = WaitingUser.Create(userId: request.UserId);
            await waitingUserRepository.AddAsync(waitingUser);
        }
        else if (!existingWaitingUser.IsWaitingForAssignment)
        {
            existingWaitingUser.MarkAsWaiting();
            await waitingUserRepository.UpdateAsync(existingWaitingUser);
        }

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            // DB-level duplicate guard: fires when Redis is down and a retry slips through
            throw new DuplicateIdempotencyKeyException(request.IdempotencyKey ?? Guid.Empty);
        }

        return new PaymentProcessResponse
        {
            Success   = true,
            PaymentId = payment.Id,
            Message   = "Payment processed successfully. User added to waiting list for coach assignment."
        };
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
        ex.InnerException?.Message.Contains("23505") == true ||
        ex.InnerException?.Message.Contains("UX_Payments_UserId_IdempotencyKey") == true;
}
