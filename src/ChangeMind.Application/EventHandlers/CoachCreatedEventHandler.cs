namespace ChangeMind.Application.EventHandlers;

using ChangeMind.Application.Events;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Events;

public sealed class CoachCreatedEventHandler(IEmailService emailService) : IEventHandler<CoachCreatedEvent>
{
    public async Task HandleAsync(CoachCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        await emailService.SendAsync(
            @event.Email,
            "Coach Account Created",
            $"Your coach account has been successfully created. Specialization: {@event.Specialization}",
            cancellationToken);
    }
}
