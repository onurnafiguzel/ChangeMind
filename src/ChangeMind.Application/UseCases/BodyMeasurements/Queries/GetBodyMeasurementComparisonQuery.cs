namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetBodyMeasurementComparisonQuery(Guid UserId) : IRequest<BodyMeasurementComparisonDto>;
