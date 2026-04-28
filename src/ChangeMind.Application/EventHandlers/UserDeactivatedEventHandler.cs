namespace ChangeMind.Application.EventHandlers;

using ChangeMind.Application.Events;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Events;

public sealed class UserDeactivatedEventHandler(IEmailService emailService) : IEventHandler<UserDeactivatedEvent>
{
    public async Task HandleAsync(UserDeactivatedEvent @event, CancellationToken cancellationToken = default)
    {
        await emailService.SendAsync(
            string.Empty,
            "Account Deactivated",
            $"User account {@event.UserId} has been deactivated.",
            cancellationToken);
    }
}
