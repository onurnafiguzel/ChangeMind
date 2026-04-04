namespace ChangeMind.Application.UseCases.Payments.Commands;

using MediatR;
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
        // Verify user exists
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        // Verify package exists
        var package = await packageRepository.GetByIdAsync(request.PackageId)
            ?? throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        // Create payment record
        var payment = Payment.Create(
            userId: request.UserId,
            packageId: request.PackageId,
            amount: request.Amount,
            description: request.Description);

        // Mock payment processing - always succeeds
        // In a real scenario, this would call a payment gateway (Stripe, PayPal, etc.)
        payment.MarkAsCompleted(transactionId: Guid.NewGuid().ToString());

        // Save payment to repository
        await paymentRepository.AddAsync(payment);

        // Add user to WaitingUsers table if not already there
        var existingWaitingUser = await waitingUserRepository.GetByUserIdAsync(request.UserId);
        if (existingWaitingUser == null)
        {
            var waitingUser = WaitingUser.Create(userId: request.UserId);
            await waitingUserRepository.AddAsync(waitingUser);
        }
        else if (!existingWaitingUser.IsWaitingForAssignment)
        {
            // If user was previously assigned but needs to be marked as waiting again
            existingWaitingUser.MarkAsWaiting();
            await waitingUserRepository.UpdateAsync(existingWaitingUser);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PaymentProcessResponse
        {
            Success = true,
            PaymentId = payment.Id,
            Message = "Payment processed successfully. User added to waiting list for coach assignment."
        };
    }
}
