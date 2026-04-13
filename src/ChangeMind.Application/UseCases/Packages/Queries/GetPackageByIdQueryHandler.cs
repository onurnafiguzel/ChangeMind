namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetPackageByIdQueryHandler(IPackageRepository packageRepository) : IRequestHandler<GetPackageByIdQuery, PackageDto>
{
    public async Task<PackageDto> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
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

        return dto;
    }
}
