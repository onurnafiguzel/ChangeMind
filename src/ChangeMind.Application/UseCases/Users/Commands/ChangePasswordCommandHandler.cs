namespace ChangeMind.Application.UseCases.Users.Commands;

using System.Security.Cryptography;
using System.Text;
using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        // Hash both passwords for comparison
        var currentPasswordHash = HashPassword(request.CurrentPassword);
        var newPasswordHash = HashPassword(request.NewPassword);

        // Verify current password
        if (user.PasswordHash != currentPasswordHash)
        {
            throw new UnauthorizedException("Current password is incorrect.");
        }

        // Change password
        user.ChangePassword(newPasswordHash);

        await userRepository.UpdateAsync(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
