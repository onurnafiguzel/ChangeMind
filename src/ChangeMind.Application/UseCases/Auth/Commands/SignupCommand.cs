namespace ChangeMind.Application.UseCases.Auth.Commands;

using MediatR;
using ChangeMind.Application.DTOs;

public record SignupCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<AuthTokenResponse>;
