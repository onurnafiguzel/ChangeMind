namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class UserPhotoRepository(ChangeMindDbContext context) : IUserPhotoRepository
{
    public async Task AddRangeAsync(IEnumerable<UserPhoto> photos)
        => await context.UserPhotos.AddRangeAsync(photos);

    public async Task<IReadOnlyList<UserPhoto>> GetByUserIdAsync(Guid userId)
        => await context.UserPhotos
            .Where(p => p.UserId == userId)
            .ToListAsync();

    public Task DeleteRangeAsync(IEnumerable<UserPhoto> photos)
    {
        context.UserPhotos.RemoveRange(photos);
        return Task.CompletedTask;
    }
}
