namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

/// <summary>
/// Register a new user with email and password only.
/// Profile information should be completed via CompleteProfileCommand.
/// </summary>
public record CreateUserCommand(
    string Email,
    string Password) : IRequest<Guid>;
