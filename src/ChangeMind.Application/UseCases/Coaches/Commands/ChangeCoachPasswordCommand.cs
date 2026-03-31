namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;

public record ChangeCoachPasswordCommand(
    Guid CoachId,
    string CurrentPassword,
    string NewPassword) : IRequest;
