namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;

public class CreatePackageCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string Type { get; set; } = string.Empty;
}
