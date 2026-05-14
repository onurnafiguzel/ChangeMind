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
            var user = waitingUser.User;

            result.Add(new UserAssignmentDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,
                Gender = user.Gender?.ToString(),
                FitnessGoal = user.FitnessGoalId.HasValue
                    && goalNames.TryGetValue(user.FitnessGoalId.Value, out var name)
                        ? name
                        : null,
                FitnessLevel = user.FitnessLevel?.ToString(),
                CreatedAt = user.CreatedAt,
                HasTrainingProgram = waitingUser.HasTrainingProgram,
                HasNutritionPlan   = waitingUser.HasNutritionPlan
            });
        }

        return result.OrderByDescending(u => u.CreatedAt).ToList();
    }
}
