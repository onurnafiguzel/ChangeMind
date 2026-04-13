namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class ChangeCoachPasswordCommandHandler(
    ICoachRepository coachRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeCoachPasswordCommand>
{
    public async Task Handle(ChangeCoachPasswordCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId)
            ?? throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        if (!passwordService.VerifyPassword(request.CurrentPassword, coach.PasswordHash))
            throw new UnauthorizedException("Current password is incorrect.");

        var newPasswordHash = passwordService.HashPassword(request.NewPassword);
        coach.ChangePassword(newPasswordHash);
        await coachRepository.UpdateAsync(coach);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
