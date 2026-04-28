namespace ChangeMind.Application.EventHandlers;

using ChangeMind.Application.Events;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Events;

public sealed class TrainingProgramAssignedEventHandler(IEmailService emailService) : IEventHandler<TrainingProgramAssignedEvent>
{
    public async Task HandleAsync(TrainingProgramAssignedEvent @event, CancellationToken cancellationToken = default)
    {
        await emailService.SendAsync(
            string.Empty,
            "Training Program Assigned",
            $"Training program {@event.TrainingProgramId} has been assigned to user {@event.UserId}.",
            cancellationToken);
    }
}
