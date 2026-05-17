namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetBodyMeasurementByIdQuery(Guid MeasurementId) : IRequest<BodyMeasurementDto?>;
