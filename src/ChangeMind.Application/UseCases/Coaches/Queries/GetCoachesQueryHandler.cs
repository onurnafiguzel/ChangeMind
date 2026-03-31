namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetCoachesQueryHandler(ICoachRepository coachRepository) : IRequestHandler<GetCoachesQuery, PagedResult<CoachDto>>
{
    public async Task<PagedResult<CoachDto>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
    {
        var query = coachRepository.GetAll(request.IsActiveOnly);

        var total = await query.CountAsync(cancellationToken);
        var coaches = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => MapToDto(c))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)total / request.PageSize);

        return new PagedResult<CoachDto>
        {
            Data = coaches,
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }

    private static CoachDto MapToDto(Domain.Entities.Coach coach)
    {
        return new CoachDto
        {
            Id = coach.Id,
            Email = coach.Email,
            FirstName = coach.FirstName,
            LastName = coach.LastName,
            Specialization = coach.Specialization?.ToString(),
            IsActive = coach.IsActive,
            CreatedAt = coach.CreatedAt,
            UpdatedAt = coach.UpdatedAt
        };
    }
}
