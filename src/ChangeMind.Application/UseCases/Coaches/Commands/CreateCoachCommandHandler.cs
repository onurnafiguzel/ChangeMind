namespace ChangeMind.Application.UseCases.Coaches.Commands;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class CreateCoachCommandHandler(
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCoachCommand, Guid>
{
    public async Task<Guid> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
    {
        if (await coachRepository.ExistsAsync(request.Email))
            throw new ConflictException($"A coach with email '{request.Email}' already exists.");

        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new NotFoundException($"A user with email '{request.Email}' not found.");

        var coach = Coach.Create(
            request.Email,
            user.PasswordHash,
            user.FirstName,
            user.LastName,
            request.Specialization);
        
        // Kullanıcıyı deaktif hale getiriyoruz çünkü artık bir koç olarak atanacak ve kullanıcı olarak aktif olmayacak
        user.Deactivate();

        await coachRepository.AddAsync(coach);
        await userRepository.UpdateAsync(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return coach.Id;
    }
}
