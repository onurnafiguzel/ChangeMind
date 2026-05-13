namespace ChangeMind.Application.UseCases.WaitingUsers.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class GetWaitingUserStatusQueryHandler(
    IWaitingUserRepository waitingUserRepository)
    : IRequestHandler<GetWaitingUserStatusQuery, WaitingUserStatusDto>
{
    public async Task<WaitingUserStatusDto> Handle(
        GetWaitingUserStatusQuery request, CancellationToken cancellationToken)
    {
        var waitingUser = await waitingUserRepository.GetByUserIdAsync(request.UserId)
            ?? throw new NotFoundException($"No waiting record found for user '{request.UserId}'.");

        return new WaitingUserStatusDto
        {
            IsWaitingForAssignment = waitingUser.IsWaitingForAssignment,
            HasTrainingProgram     = waitingUser.HasTrainingProgram,
            HasNutritionPlan       = waitingUser.HasNutritionPlan,
            CreatedAt              = waitingUser.CreatedAt
        };
    }
}
