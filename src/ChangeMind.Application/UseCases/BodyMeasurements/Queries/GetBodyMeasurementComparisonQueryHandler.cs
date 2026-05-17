namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetBodyMeasurementComparisonQueryHandler(IBodyMeasurementRepository repository)
    : IRequestHandler<GetBodyMeasurementComparisonQuery, BodyMeasurementComparisonDto>
{
    public async Task<BodyMeasurementComparisonDto> Handle(GetBodyMeasurementComparisonQuery request, CancellationToken cancellationToken)
    {
        var latest = await repository.GetLatestTwoAsync(request.UserId);
        var result = new BodyMeasurementComparisonDto();

        // latest is ordered newest-first.
        if (latest.Count > 0) result.Current  = BodyMeasurementMapper.ToDto(latest[0]);
        if (latest.Count > 1) result.Previous = BodyMeasurementMapper.ToDto(latest[1]);

        if (result.Previous is not null && result.Current is not null)
        {
            result.Delta = new MeasurementDeltaDto
            {
                WeightKg        = Diff(result.Current.WeightKg, result.Previous.WeightKg),
                WaistCm         = Diff(result.Current.WaistCm, result.Previous.WaistCm),
                ArmCm           = Diff(result.Current.ArmCm, result.Previous.ArmCm),
                LegCm           = Diff(result.Current.LegCm, result.Previous.LegCm),
                NeckCm          = Diff(result.Current.NeckCm, result.Previous.NeckCm),
                HipCm           = Diff(result.Current.HipCm, result.Previous.HipCm),
                BodyFatPercent  = Diff(result.Current.BodyFatPercent, result.Previous.BodyFatPercent)
            };
        }

        return result;
    }

    private static decimal? Diff(decimal? current, decimal? previous)
        => current is null || previous is null ? null : Math.Round(current.Value - previous.Value, 2);
}
