namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Entities;

public class CreateCoachCommandHandler(
    ICoachRepository coachRepository,
    IPasswordService passwordService) : IRequestHandler<CreateCoachCommand, Guid>
{
    public async Task<Guid> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
    {
        if (await coachRepository.ExistsAsync(request.Email))
            throw new InvalidOperationException($"A coach with email '{request.Email}' already exists.");

        var passwordHash = passwordService.HashPassword(request.Password);
        var coach = Coach.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.Specialization);

        await coachRepository.AddAsync(coach);
        return coach.Id;
    }
}
