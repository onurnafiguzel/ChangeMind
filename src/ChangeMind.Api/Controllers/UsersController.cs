namespace ChangeMind.Api.Controllers;

using MediatR;
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
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery { UserId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new user
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
    /// Update user profile
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        var command = baseRequest with { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Soft delete user (sets IsActive = false)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(
        Guid id,
        [FromBody] ChangePasswordCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        var command = baseRequest with { UserId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
