namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using Microsoft.Extensions.Options;

public class GetPackagesQueryHandler(
    IPackageRepository packageRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions) : IRequestHandler<GetPackagesQuery, PagedResult<PackageDto>>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<PagedResult<PackageDto>> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.PackageList(request.IsActiveOnly, request.Page, request.PageSize);

        var cached = await cache.GetAsync<PagedResult<PackageDto>>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var query = packageRepository.GetAll(request.IsActiveOnly);

        var total = await query.CountAsync(cancellationToken);
        var packages = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)total / request.PageSize);

        var result = new PagedResult<PackageDto>
        {
            Data = packages,
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };

        await cache.SetAsync(key, result, TimeSpan.FromSeconds(_cacheOptions.PackageTtlSeconds), cancellationToken);

        return result;
    }
}
