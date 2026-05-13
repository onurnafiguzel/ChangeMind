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

        // Sequential — DbContext is not thread-safe; these handlers share the same scoped DbContext.
        var activeProgram = await mediator.Send(new GetUserActiveProgramQuery { UserId = request.UserId }, cancellationToken);
        var activeNutrition = await mediator.Send(new GetUserActiveNutritionPlanQuery(request.UserId), cancellationToken);
        var waiting = await waitingUserRepository.GetByUserIdAsync(request.UserId);

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
            ActiveTrainingProgram = activeProgram,
            ActiveNutritionPlan   = activeNutrition,
            WaitingStatus         = waitingStatus
        };
    }
}
