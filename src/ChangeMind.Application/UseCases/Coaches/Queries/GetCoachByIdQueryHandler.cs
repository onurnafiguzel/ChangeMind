namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetCoachByIdQueryHandler(ICoachRepository coachRepository) : IRequestHandler<GetCoachByIdQuery, CoachDto>
{
    public async Task<CoachDto> Handle(GetCoachByIdQuery request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId);
        if (coach == null)
            throw new KeyNotFoundException($"Coach with ID '{request.CoachId}' not found.");

        return MapToDto(coach);
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
