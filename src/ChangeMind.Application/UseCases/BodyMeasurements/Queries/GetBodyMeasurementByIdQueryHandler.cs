namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetBodyMeasurementByIdQueryHandler(IBodyMeasurementRepository repository)
    : IRequestHandler<GetBodyMeasurementByIdQuery, BodyMeasurementDto?>
{
    public async Task<BodyMeasurementDto?> Handle(GetBodyMeasurementByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.MeasurementId);
        return entity is null ? null : BodyMeasurementMapper.ToDto(entity);
    }
}
