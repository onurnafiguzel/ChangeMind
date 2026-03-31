namespace ChangeMind.Application.UseCases.Auth.Commands;

using MediatR;
using ChangeMind.Application.DTOs;

public record LoginCommand(string Email, string Password) : IRequest<AuthTokenResponse>;
