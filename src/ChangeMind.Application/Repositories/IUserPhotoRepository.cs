namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IUserPhotoRepository
{
    Task AddRangeAsync(IEnumerable<UserPhoto> photos);
    Task<IReadOnlyList<UserPhoto>> GetByUserIdAsync(Guid userId);
    Task DeleteRangeAsync(IEnumerable<UserPhoto> photos);
}
