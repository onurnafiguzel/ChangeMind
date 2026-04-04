namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetWaitingUsersQuery : IRequest<List<UserAssignmentDto>>
{
    /// <summary>
    /// Get users waiting for coach assignment.
    /// These are users with completed payments and IsWaitingForAssignment = true.
    /// </summary>
}
