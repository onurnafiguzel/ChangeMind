namespace ChangeMind.Application.UseCases.Users.Commands;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await userRepository.ExistsAsync(request.Email))
            throw new ConflictException($"User with email '{request.Email}' already exists.");
        // Hash password with SHA256
        var passwordHash = passwordService.HashPassword(request.Password);

        // Create user with email and password only (registration)
        // Profile information will be completed via CompleteProfileCommand
        var user = User.Create(
            email: request.Email,
            passwordHash: passwordHash);

        await userRepository.AddAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
