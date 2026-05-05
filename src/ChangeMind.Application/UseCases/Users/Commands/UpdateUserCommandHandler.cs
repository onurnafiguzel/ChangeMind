namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<UpdateUserCommand>
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
            fitnessGoalId: request.FitnessGoal,
            fitnessLevel: request.FitnessLevel);

        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(CacheKeys.User(request.UserId), cancellationToken);
    }
}
