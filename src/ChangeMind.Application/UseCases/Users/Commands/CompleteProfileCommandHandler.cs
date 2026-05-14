namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class CompleteProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<CompleteProfileCommand>
{
    public async Task Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        user.CompleteProfile(
            firstName:              request.FirstName,
            lastName:               request.LastName,
            age:                    request.Age,
            height:                 request.Height,
            weight:                 request.Weight,
            gender:                 request.Gender,
            fitnessGoalId:          request.FitnessGoalId,
            fitnessLevel:           request.FitnessLevel,
            dailyWorkLifestyle:     request.DailyWorkLifestyle,
            gymDaysPerWeek:         request.GymDaysPerWeek,
            healthConditions:       request.HealthConditions,
            foodAllergies:          request.FoodAllergies,
            supplementInterest:     request.SupplementInterest,
            wantsSupplementSupport: request.WantsSupplementSupport);

        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(CacheKeys.User(request.UserId), cancellationToken);
    }
}
