namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetPackageByIdQueryHandler(IPackageRepository packageRepository) : IRequestHandler<GetPackageByIdQuery, PackageDto>
{
    public async Task<PackageDto> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId);
        if (package == null)
            throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        return new PackageDto
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description,
            Price = package.Price,
            DurationDays = package.DurationDays,
            Type = package.Type.ToString(),
            IsActive = package.IsActive,
            CreatedAt = package.CreatedAt,
            UpdatedAt = package.UpdatedAt
        };
    }
}
