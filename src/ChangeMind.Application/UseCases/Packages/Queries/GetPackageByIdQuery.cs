namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetPackageByIdQuery : IRequest<PackageDto>
{
    public Guid PackageId { get; set; }
}
