namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;

public record UpdatePackageCommand : IRequest<Unit>
{
    public Guid PackageId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int DurationDays { get; init; }
    public string Type { get; init; } = string.Empty;
}
