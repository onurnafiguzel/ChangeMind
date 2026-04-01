namespace ChangeMind.Application.UseCases.Coaches.Commands;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Entities;
using MediatR;

public class CreateCoachCommandHandler(
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IPasswordService passwordService) : IRequestHandler<CreateCoachCommand, Guid>
{
    public async Task<Guid> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
    {
        if (await coachRepository.ExistsAsync(request.Email))
            throw new InvalidOperationException($"A coach with email '{request.Email}' already exists.");

        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new InvalidOperationException($"A user with email '{request.Email}' not found.");

        var coach = Coach.Create(
            request.Email,
            user.PasswordHash,
            user.FirstName,
            user.LastName,
            request.Specialization);
        
        // Kullanýcýyý deaktif hale getiriyoruz çünkü artýk bir koç olarak atanacak ve kullanýcý olarak aktif olmayacak
        user.Deactivate();

        await coachRepository.AddAsync(coach);
        return coach.Id;
    }
}
