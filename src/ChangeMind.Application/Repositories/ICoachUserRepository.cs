namespace ChangeMind.Application.Repositories;

public interface ICoachUserRepository
{
    Task<bool> IsActiveAssignmentAsync(Guid coachId, Guid userId);
}
