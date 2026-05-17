namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;

public interface IPersonalRecordRepository
{
    Task AddAsync(PersonalRecord record);
    Task<IReadOnlyList<PersonalRecord>> GetCurrentByUserAsync(Guid userId);
    Task<IReadOnlyList<PersonalRecord>> GetHistoryAsync(Guid userId, PersonalRecordLift lift);
}
