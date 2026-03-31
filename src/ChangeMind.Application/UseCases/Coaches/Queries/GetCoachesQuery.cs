namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetCoachesQuery : IRequest<PagedResult<CoachDto>>
{
    public bool? IsActiveOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
