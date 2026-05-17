namespace ChangeMind.Application.UseCases.BodyMeasurements.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using ChangeMind.Domain.Services;

public class AddBodyMeasurementCommandHandler(
    IUserRepository userRepository,
    IBodyMeasurementRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddBodyMeasurementCommand, Guid>
{
    public async Task<Guid> Handle(AddBodyMeasurementCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        var bodyFat = BodyFatCalculator.CalculateUsNavy(
            user.Profile?.Gender,
            user.Profile?.Height,
            request.WaistCm,
            request.NeckCm,
            request.HipCm);

        var measurement = BodyMeasurement.Create(
            userId:         request.UserId,
            recordedAt:     request.RecordedAt ?? DateTime.UtcNow,
            weightKg:       request.WeightKg,
            waistCm:        request.WaistCm,
            armCm:          request.ArmCm,
            legCm:          request.LegCm,
            neckCm:         request.NeckCm,
            hipCm:          request.HipCm,
            bodyFatPercent: bodyFat,
            notes:          request.Notes);

        await repository.AddAsync(measurement);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return measurement.Id;
    }
}
