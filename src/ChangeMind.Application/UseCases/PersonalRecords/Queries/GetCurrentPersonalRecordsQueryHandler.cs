namespace ChangeMind.Application.UseCases.PersonalRecords.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Enums;

public class GetCurrentPersonalRecordsQueryHandler(IPersonalRecordRepository repository)
    : IRequestHandler<GetCurrentPersonalRecordsQuery, List<PersonalRecordCurrentDto>>
{
    private static readonly PersonalRecordLift[] AllLifts =
        Enum.GetValues<PersonalRecordLift>();

    public async Task<List<PersonalRecordCurrentDto>> Handle(GetCurrentPersonalRecordsQuery request, CancellationToken cancellationToken)
    {
        var records = await repository.GetCurrentByUserAsync(request.UserId);
        var byLift = records.GroupBy(r => r.Lift)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(r => r.WeightKg).First());

        return AllLifts.Select(l =>
        {
            byLift.TryGetValue(l, out var pr);
            return new PersonalRecordCurrentDto
            {
                Lift       = l,
                WeightKg   = pr?.WeightKg,
                RecordedAt = pr?.RecordedAt
            };
        }).ToList();
    }
}
