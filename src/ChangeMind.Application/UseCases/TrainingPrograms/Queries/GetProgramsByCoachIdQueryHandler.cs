namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetProgramsByCoachIdQueryHandler(
    ITrainingProgramRepository trainingProgramRepository)
    : IRequestHandler<GetProgramsByCoachIdQuery, List<CoachProgramListItemDto>>
{
    public async Task<List<CoachProgramListItemDto>> Handle(
        GetProgramsByCoachIdQuery request,
        CancellationToken cancellationToken)
    {
        var programs = await trainingProgramRepository.GetActiveByCoachIdAsync(request.CoachId);

        return programs.Select(p => new CoachProgramListItemDto
        {
            Id                 = p.Id,
            Name               = p.Name,
            Description        = p.Description,
            DurationWeeks      = p.DurationWeeks,
            CompletedWeeks     = p.CompletedWeeks,
            ProgressPercentage = p.ProgressPercentage,
            Difficulty         = p.Difficulty,
            StartDate          = p.StartDate,
            EndDate            = p.EndDate,
            CreatedAt          = p.CreatedAt,
            IsCompleted        = p.IsCompleted,
            UserId             = p.UserId,
            UserAge            = p.AssignedTo.Profile?.Age,
            UserHeight         = p.AssignedTo.Profile?.Height,
            UserWeight         = p.AssignedTo.Profile?.Weight,
            UserGender         = p.AssignedTo.Profile?.Gender
        }).ToList();
    }
}
