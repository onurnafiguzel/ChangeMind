namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetPackagesQueryHandler(IPackageRepository packageRepository) : IRequestHandler<GetPackagesQuery, PagedResult<PackageDto>>
{
    public async Task<PagedResult<PackageDto>> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
    {
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

        return new PagedResult<PackageDto>
        {
            Data = packages,
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
}
