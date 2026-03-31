namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Auth.Commands;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// User signup (registration)
    /// </summary>
    [HttpPost("signup")]
    public async Task<ActionResult<AuthTokenResponse>> Signup(
        [FromBody] SignupCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// User login
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenResponse>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}
