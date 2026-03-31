namespace ChangeMind.Application.UseCases.Coaches.Commands;

using System.Security.Cryptography;
using System.Text;
using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;

public class CreateCoachCommandHandler(ICoachRepository coachRepository) : IRequestHandler<CreateCoachCommand, Guid>
{
    public async Task<Guid> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
    {
        if (await coachRepository.ExistsAsync(request.Email))
            throw new InvalidOperationException($"A coach with email '{request.Email}' already exists.");

        var passwordHash = HashPassword(request.Password);
        var coach = Coach.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.Specialization);

        await coachRepository.AddAsync(coach);
        return coach.Id;
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
