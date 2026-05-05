namespace ChangeMind.Application.UseCases.FitnessGoals.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetFitnessGoalsQueryHandler(IFitnessGoalRepository repository)
    : IRequestHandler<GetFitnessGoalsQuery, List<FitnessGoalDto>>
{
    public async Task<List<FitnessGoalDto>> Handle(
        GetFitnessGoalsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll()
            .OrderBy(f => f.Name)
            .Select(f => new FitnessGoalDto
            {
                Id          = f.Id,
                Name        = f.Name,
                Description = f.Description,
                IsActive    = f.IsActive,
                CreatedAt   = f.CreatedAt,
                UpdatedAt   = f.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
