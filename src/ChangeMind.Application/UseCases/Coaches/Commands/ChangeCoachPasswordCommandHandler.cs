namespace ChangeMind.Application.UseCases.Coaches.Commands;

using System.Security.Cryptography;
using System.Text;
using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class ChangeCoachPasswordCommandHandler(
    ICoachRepository coachRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeCoachPasswordCommand>
{
    public async Task Handle(ChangeCoachPasswordCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId);
        if (coach == null)
            throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        var currentPasswordHash = HashPassword(request.CurrentPassword);
        if (coach.PasswordHash != currentPasswordHash)
            throw new UnauthorizedException("Current password is incorrect.");

        var newPasswordHash = HashPassword(request.NewPassword);
        coach.ChangePassword(newPasswordHash);
        await coachRepository.UpdateAsync(coach);

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
