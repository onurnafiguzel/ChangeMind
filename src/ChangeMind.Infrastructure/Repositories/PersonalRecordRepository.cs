namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Infrastructure.Data;

public class PersonalRecordRepository(ChangeMindDbContext context) : IPersonalRecordRepository
{
    public async Task AddAsync(PersonalRecord record)
    {
        await context.PersonalRecords.AddAsync(record);
    }

    public async Task<IReadOnlyList<PersonalRecord>> GetCurrentByUserAsync(Guid userId)
    {
        // En yüksek ağırlık per lift
        return await context.PersonalRecords
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .GroupBy(p => p.Lift)
            .Select(g => g.OrderByDescending(p => p.WeightKg).ThenByDescending(p => p.RecordedAt).First())
            .ToListAsync();
    }

    public async Task<IReadOnlyList<PersonalRecord>> GetHistoryAsync(Guid userId, PersonalRecordLift lift)
    {
        return await context.PersonalRecords
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.Lift == lift)
            .OrderByDescending(p => p.RecordedAt)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
