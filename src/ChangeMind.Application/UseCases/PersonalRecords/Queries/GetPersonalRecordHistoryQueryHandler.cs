namespace ChangeMind.Application.UseCases.PersonalRecords.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetPersonalRecordHistoryQueryHandler(IPersonalRecordRepository repository)
    : IRequestHandler<GetPersonalRecordHistoryQuery, List<PersonalRecordDto>>
{
    public async Task<List<PersonalRecordDto>> Handle(GetPersonalRecordHistoryQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetHistoryAsync(request.UserId, request.Lift);
        return items.Select(pr => new PersonalRecordDto
        {
            Id          = pr.Id,
            UserId      = pr.UserId,
            Lift        = pr.Lift,
            WeightKg    = pr.WeightKg,
            RecordedAt  = pr.RecordedAt,
            CreatedAt   = pr.CreatedAt
        }).ToList();
    }
}
