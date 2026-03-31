namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.DTOs;

public record LoginCoachCommand(string Email, string Password) : IRequest<AuthTokenResponse>;
