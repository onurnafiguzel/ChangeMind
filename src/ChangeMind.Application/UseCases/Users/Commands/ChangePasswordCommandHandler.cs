namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        if (!passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedException("Current password is incorrect.");

        var newPasswordHash = passwordService.HashPassword(request.NewPassword);
        user.ChangePassword(newPasswordHash);

        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
