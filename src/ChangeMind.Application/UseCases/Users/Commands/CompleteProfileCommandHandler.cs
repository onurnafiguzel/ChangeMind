namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class CompleteProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CompleteProfileCommand>
{
    public async Task Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
    {
        // Get user by ID
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        // Complete profile with personal and fitness information
        user.CompleteProfile(
            firstName: request.FirstName,
            lastName: request.LastName,
            age: request.Age,
            height: request.Height,
            weight: request.Weight,
            gender: request.Gender,
            fitnessGoal: request.FitnessGoal,
            fitnessLevel: request.FitnessLevel);

        // Update repository
        await userRepository.UpdateAsync(user);

        // Save all changes in a single transaction
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
