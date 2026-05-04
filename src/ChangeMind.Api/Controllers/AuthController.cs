namespace ChangeMind.Api.Controllers;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Login — works for both Users and Coaches.
    /// Returns an access token and refresh token.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenResponse>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Register a new user account and return tokens (auto-login).
    /// Profile details should be completed via PATCH /api/users/{id}/complete-profile.
    /// </summary>
    [HttpPost("signup")]
    public async Task<ActionResult<AuthTokenResponse>> Signup(
        [FromBody] SignupCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Login), result);
    }
}
