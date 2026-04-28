namespace ChangeMind.Application.EventHandlers;

using ChangeMind.Application.Events;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Events;

public sealed class UserCreatedEventHandler(IEmailService emailService) : IEventHandler<UserCreatedEvent>
{
    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        await emailService.SendWelcomeEmailAsync(
            @event.Email,
            string.Empty,
            cancellationToken);
    }
}
