namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Exceptions;
using Microsoft.Extensions.Options;

public class GetPackageByIdQueryHandler(
    IPackageRepository packageRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions) : IRequestHandler<GetPackageByIdQuery, PackageDto>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<PackageDto> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.Package(request.PackageId);

        var cached = await cache.GetAsync<PackageDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await packageRepository.GetById(request.PackageId)
            .Select(p => new PackageDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DurationDays = p.DurationDays,
                Type = p.Type.ToString(),
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        await cache.SetAsync(key, dto, TimeSpan.FromSeconds(_cacheOptions.PackageTtlSeconds), cancellationToken);

        return dto;
    }
}
