namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;

public class DeletePackageCommand : IRequest<Unit>
{
    public Guid PackageId { get; set; }
}
