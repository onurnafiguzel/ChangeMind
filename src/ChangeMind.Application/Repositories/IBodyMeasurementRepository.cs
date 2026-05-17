namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IBodyMeasurementRepository
{
    Task AddAsync(BodyMeasurement measurement);
    Task<BodyMeasurement?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<BodyMeasurement>> GetByUserAsync(Guid userId, int take);
    Task<IReadOnlyList<BodyMeasurement>> GetLatestTwoAsync(Guid userId);
}
