namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetWaitingUsersQueryHandler(
    IWaitingUserRepository waitingUserRepository,
    IFitnessGoalRepository fitnessGoalRepository) : IRequestHandler<GetWaitingUsersQuery, List<UserAssignmentDto>>
{
    public async Task<List<UserAssignmentDto>> Handle(GetWaitingUsersQuery request, CancellationToken cancellationToken)
    {
        var waitingUsers = await waitingUserRepository
            .GetWaitingForAssignment()
            .ToListAsync(cancellationToken);

        var goalNames = await fitnessGoalRepository
            .GetAll()
            .ToDictionaryAsync(g => g.Id, g => g.Name, cancellationToken);

        var result = new List<UserAssignmentDto>();

        foreach (var waitingUser in waitingUsers)
        {
            var user    = waitingUser.User;
            var profile = user.Profile;

            result.Add(new UserAssignmentDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = profile?.FirstName ?? string.Empty,
                LastName = profile?.LastName ?? string.Empty,
                Age = profile?.Age,
                Height = profile?.Height,
                Weight = profile?.Weight,
                Gender = profile?.Gender?.ToString(),
                FitnessGoal = profile?.FitnessGoalId is { } goalId
                    && goalNames.TryGetValue(goalId, out var name)
                        ? name
                        : null,
                FitnessLevel = profile?.FitnessLevel?.ToString(),
                CreatedAt = user.CreatedAt,
                HasTrainingProgram = waitingUser.HasTrainingProgram,
                HasNutritionPlan   = waitingUser.HasNutritionPlan
            });
        }

        return result.OrderByDescending(u => u.CreatedAt).ToList();
    }
}
