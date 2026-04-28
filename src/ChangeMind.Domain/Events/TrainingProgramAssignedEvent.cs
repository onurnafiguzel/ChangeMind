namespace ChangeMind.Domain.Events;

public sealed class TrainingProgramAssignedEvent(Guid trainingProgramId, Guid userId) : DomainEvent
{
    public Guid TrainingProgramId { get; } = trainingProgramId;
    public Guid UserId { get; } = userId;
}
