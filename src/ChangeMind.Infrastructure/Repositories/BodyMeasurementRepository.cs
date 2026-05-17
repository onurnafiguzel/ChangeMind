namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class BodyMeasurementRepository(ChangeMindDbContext context) : IBodyMeasurementRepository
{
    public async Task AddAsync(BodyMeasurement measurement)
    {
        await context.BodyMeasurements.AddAsync(measurement);
    }

    public async Task<BodyMeasurement?> GetByIdAsync(Guid id)
    {
        return await context.BodyMeasurements
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IReadOnlyList<BodyMeasurement>> GetByUserAsync(Guid userId, int take)
    {
        return await context.BodyMeasurements
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.RecordedAt)
            .ThenByDescending(m => m.CreatedAt)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<BodyMeasurement>> GetLatestTwoAsync(Guid userId)
    {
        return await context.BodyMeasurements
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.RecordedAt)
            .ThenByDescending(m => m.CreatedAt)
            .Take(2)
            .ToListAsync();
    }
}
