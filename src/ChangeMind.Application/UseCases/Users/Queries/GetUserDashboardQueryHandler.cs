namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.NutritionPlans.Queries;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;
using ChangeMind.Domain.Exceptions;

public class GetUserDashboardQueryHandler(
    IUserRepository userRepository,
    IWaitingUserRepository waitingUserRepository,
    IMediator mediator)
    : IRequestHandler<GetUserDashboardQuery, UserDashboardDto>
{
    public async Task<UserDashboardDto> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User '{request.UserId}' not found.");

        var trainingTask = mediator.Send(new GetUserActiveProgramQuery { UserId = request.UserId }, cancellationToken);
        var nutritionTask = mediator.Send(new GetUserActiveNutritionPlanQuery(request.UserId), cancellationToken);
        var waitingTask = waitingUserRepository.GetByUserIdAsync(request.UserId);

        await Task.WhenAll(trainingTask, nutritionTask, waitingTask);

        var waiting = await waitingTask;
        WaitingUserStatusDto? waitingStatus = waiting is null ? null : new WaitingUserStatusDto
        {
            IsWaitingForAssignment = waiting.IsWaitingForAssignment,
            HasTrainingProgram     = waiting.HasTrainingProgram,
            HasNutritionPlan       = waiting.HasNutritionPlan,
            CreatedAt              = waiting.CreatedAt
        };

        return new UserDashboardDto
        {
            Profile = new UserDashboardProfileDto
            {
                Id                 = user.Id,
                Email              = user.Email,
                FirstName          = user.FirstName,
                LastName           = user.LastName,
                Age                = user.Age,
                Height             = user.Height,
                Weight             = user.Weight,
                Gender             = user.Gender?.ToString(),
                FitnessLevel       = user.FitnessLevel?.ToString(),
                IsCompletedProfile = user.IsCompletedProfile
            },
            ActiveTrainingProgram = await trainingTask,
            ActiveNutritionPlan   = await nutritionTask,
            WaitingStatus         = waitingStatus
        };
    }
}
