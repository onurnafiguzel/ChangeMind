namespace ChangeMind.Domain.Events;

using ChangeMind.Domain.Enums;

public sealed class CoachCreatedEvent(Guid coachId, string email, CoachSpecialization? specialization) : DomainEvent
{
    public Guid CoachId { get; } = coachId;
    public string Email { get; } = email;
    public CoachSpecialization? Specialization { get; } = specialization;
}
