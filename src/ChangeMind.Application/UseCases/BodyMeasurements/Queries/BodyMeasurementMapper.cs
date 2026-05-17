namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Entities;

internal static class BodyMeasurementMapper
{
    public static BodyMeasurementDto ToDto(BodyMeasurement m) => new()
    {
        Id              = m.Id,
        UserId          = m.UserId,
        RecordedAt      = m.RecordedAt,
        WeightKg        = m.WeightKg,
        WaistCm         = m.WaistCm,
        ArmCm           = m.ArmCm,
        LegCm           = m.LegCm,
        NeckCm          = m.NeckCm,
        HipCm           = m.HipCm,
        BodyFatPercent  = m.BodyFatPercent,
        Notes           = m.Notes,
        CreatedAt       = m.CreatedAt
    };
}
