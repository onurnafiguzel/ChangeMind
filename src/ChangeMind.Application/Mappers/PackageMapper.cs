namespace ChangeMind.Application.Mappers;

using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Entities;

public static class PackageMapper
{
    public static PackageDto ToDto(Package package) => new()
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
