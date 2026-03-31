namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;

public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword) : IRequest;
