namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetUserBodyMeasurementsQuery(Guid UserId, int Take = 20) : IRequest<List<BodyMeasurementDto>>;
