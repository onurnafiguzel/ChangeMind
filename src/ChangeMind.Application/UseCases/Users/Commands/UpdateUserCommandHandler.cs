namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        user.Update(
            firstName: request.FirstName,
            lastName: request.LastName,
            age: request.Age,
            height: request.Height,
            weight: request.Weight,
            gender: request.Gender,
            fitnessGoal: request.FitnessGoal,
            fitnessLevel: request.FitnessLevel);

        await userRepository.UpdateAsync(user);

        // Save all changes in a single transaction
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
