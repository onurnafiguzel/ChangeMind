namespace ChangeMind.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Users.Queries;
using ChangeMind.Application.UseCases.WaitingUsers.Queries;
using ChangeMind.Domain.Exceptions;

[ApiController]
[Route("api/waiting-users")]
[Authorize]
public class WaitingUsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// List all users waiting for coach assignment (Coach or Admin only).
    /// </summary>
    [HttpGet]
    [RequestTimeout("bulk-list")]
    public async Task<ActionResult<List<UserAssignmentDto>>> GetWaitingUsers(
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetWaitingUsersQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get waiting status for a specific user.
    /// Users can only query their own status; coaches and admins may query any user.
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<WaitingUserStatusDto>> GetWaitingUserStatus(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var tokenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role        = User.FindFirstValue(ClaimTypes.Role);

        var isPrivileged = role is "Coach" or "Admin";

        if (!isPrivileged)
        {
            if (!Guid.TryParse(tokenUserId, out var tokenGuid) || tokenGuid != userId)
                throw new ForbiddenException("You can only query your own waiting status.");
        }

        var result = await mediator.Send(new GetWaitingUserStatusQuery(userId), cancellationToken);
        return Ok(result);
    }
}
