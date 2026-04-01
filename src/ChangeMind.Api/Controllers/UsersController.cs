namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Users.Commands;
using ChangeMind.Application.UseCases.Users.Queries;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Get all users with optional pagination and active filter
    /// </summary>
    [Authorize(Roles = "Admin,Coach")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] bool? isActiveOnly = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery
        {
            IsActiveOnly = isActiveOnly,
            Page = page,
            PageSize = pageSize
        };

        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id, CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(id))
            return Forbid();

        var query = new GetUserByIdQuery { UserId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Register a new user with email and password
    /// Complete profile information separately using PUT /api/users/{id}/complete-profile
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
    }

    /// <summary>
    /// Complete user profile with personal and fitness information (called after registration)
    /// </summary>
    [Authorize]
    [HttpPost("{id:guid}/complete-profile")]
    public async Task<IActionResult> CompleteProfile(
        Guid id,
        [FromBody] CompleteProfileCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(id))
            return Forbid();

        var command = baseRequest with { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(id))
            return Forbid();

        var command = baseRequest with { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete user (sets IsActive = false)
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(id))
            return Forbid();

        var command = new DeleteUserCommand { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [Authorize]
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(
        Guid id,
        [FromBody] ChangePasswordCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthorizedForUser(id))
            return Forbid();

        var command = baseRequest with { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    private bool IsAuthorizedForUser(Guid userId)
    {
        // Get userId from JWT token claims
        var tokenUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        // Allow if user ID matches OR user is Admin
        if (Guid.TryParse(tokenUserIdClaim, out var tokenUserId))
        {
            return tokenUserId == userId || userRoleClaim == "Admin";
        }

        return false;
    }
}
