namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetCoachByIdQueryHandler(ICoachRepository coachRepository) : IRequestHandler<GetCoachByIdQuery, CoachDto>
{
    public async Task<CoachDto> Handle(GetCoachByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await coachRepository.GetById(request.CoachId)
            .Select(c => new CoachDto
            {
                Id = c.Id,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Specialization = c.Specialization.ToString(),
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        return dto;
    }
}
