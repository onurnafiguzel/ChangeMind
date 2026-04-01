namespace ChangeMind.Application.UseCases.Users.Commands;

using System.Security.Cryptography;
using System.Text;
using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await userRepository.ExistsAsync(request.Email))
        {
            throw new ConflictException($"User with email '{request.Email}' already exists.");
        }

        // Hash password with SHA256
        var passwordHash = HashPassword(request.Password);

        // Create user via factory method
        var user = User.Create(
            email: request.Email,
            passwordHash: passwordHash,
            firstName: request.FirstName,
            lastName: request.LastName,
            age: request.Age,
            height: request.Height,
            weight: request.Weight,
            gender: request.Gender,
            fitnessGoal: request.FitnessGoal,
            fitnessLevel: request.FitnessLevel);

        // Add to repository
        await userRepository.AddAsync(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
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
