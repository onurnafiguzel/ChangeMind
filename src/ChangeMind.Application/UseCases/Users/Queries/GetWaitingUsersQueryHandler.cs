namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetWaitingUsersQueryHandler(
    IWaitingUserRepository waitingUserRepository,
    IPaymentRepository paymentRepository) : IRequestHandler<GetWaitingUsersQuery, List<UserAssignmentDto>>
{
    public async Task<List<UserAssignmentDto>> Handle(GetWaitingUsersQuery request, CancellationToken cancellationToken)
    {
        // Get all users waiting for assignment
        var waitingUsers = await waitingUserRepository
            .GetWaitingForAssignment()
            .ToListAsync(cancellationToken);

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
                FitnessGoal = user.FitnessGoal?.ToString(),
                FitnessLevel = user.FitnessLevel?.ToString(),
                CreatedAt = user.CreatedAt
            });
        }

        return result.OrderByDescending(u => u.CreatedAt).ToList();
    }
}
