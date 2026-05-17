namespace ChangeMind.Application.UseCases.BodyMeasurements.Commands;

using MediatR;

public record AddBodyMeasurementCommand(
    Guid UserId,
    DateTime? RecordedAt,
    decimal? WeightKg,
    decimal? WaistCm,
    decimal? ArmCm,
    decimal? LegCm,
    decimal? NeckCm,
    decimal? HipCm,
    string? Notes) : IRequest<Guid>;
