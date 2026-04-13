namespace ChangeMind.Application.UseCases.Packages.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetPackagesQuery : IRequest<PagedResult<PackageDto>>
{
    public bool? IsActiveOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
