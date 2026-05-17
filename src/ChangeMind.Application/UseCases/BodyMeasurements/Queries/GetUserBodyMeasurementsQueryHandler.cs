namespace ChangeMind.Application.UseCases.BodyMeasurements.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserBodyMeasurementsQueryHandler(IBodyMeasurementRepository repository)
    : IRequestHandler<GetUserBodyMeasurementsQuery, List<BodyMeasurementDto>>
{
    public async Task<List<BodyMeasurementDto>> Handle(GetUserBodyMeasurementsQuery request, CancellationToken cancellationToken)
    {
        var take = Math.Clamp(request.Take, 1, 100);
        var items = await repository.GetByUserAsync(request.UserId, take);
        return items.Select(BodyMeasurementMapper.ToDto).ToList();
    }
}
