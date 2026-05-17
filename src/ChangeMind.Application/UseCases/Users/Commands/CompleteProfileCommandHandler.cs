namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using ChangeMind.Domain.Services;

public class CompleteProfileCommandHandler(
    IUserRepository userRepository,
    IBodyMeasurementRepository bodyMeasurementRepository,
    IPersonalRecordRepository personalRecordRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<CompleteProfileCommand>
{
    public async Task Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        user.CompleteProfile(
            firstName:              request.FirstName,
            lastName:               request.LastName,
            age:                    request.Age,
            height:                 request.Height,
            weight:                 request.Weight,
            gender:                 request.Gender,
            fitnessGoalId:          request.FitnessGoalId,
            fitnessLevel:           request.FitnessLevel,
            dailyWorkLifestyle:     request.DailyWorkLifestyle,
            gymDaysPerWeek:         request.GymDaysPerWeek,
            healthConditions:       request.HealthConditions,
            foodAllergies:          request.FoodAllergies,
            supplementInterest:     request.SupplementInterest,
            wantsSupplementSupport: request.WantsSupplementSupport);

        await userRepository.UpdateAsync(user);

        var hasInitialMeasurement =
            request.WaistCm.HasValue || request.ArmCm.HasValue || request.LegCm.HasValue ||
            request.NeckCm.HasValue || request.HipCm.HasValue || request.Weight.HasValue;

        if (hasInitialMeasurement)
        {
            var bodyFat = BodyFatCalculator.CalculateUsNavy(
                request.Gender, request.Height, request.WaistCm, request.NeckCm, request.HipCm);

            var measurement = BodyMeasurement.Create(
                userId:         request.UserId,
                recordedAt:     DateTime.UtcNow,
                weightKg:       request.Weight,
                waistCm:        request.WaistCm,
                armCm:          request.ArmCm,
                legCm:          request.LegCm,
                neckCm:         request.NeckCm,
                hipCm:          request.HipCm,
                bodyFatPercent: bodyFat,
                notes:          null);

            await bodyMeasurementRepository.AddAsync(measurement);
        }

        await AddPrIfPresent(request.UserId, PersonalRecordLift.BenchPress,    request.BenchPressPR);
        await AddPrIfPresent(request.UserId, PersonalRecordLift.Squat,         request.SquatPR);
        await AddPrIfPresent(request.UserId, PersonalRecordLift.Deadlift,      request.DeadliftPR);
        await AddPrIfPresent(request.UserId, PersonalRecordLift.OverheadPress, request.OverheadPressPR);
        await AddPrIfPresent(request.UserId, PersonalRecordLift.BarbellRow,    request.BarbellRowPR);
        await AddPrIfPresent(request.UserId, PersonalRecordLift.PullUp,        request.PullUpPR);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(CacheKeys.User(request.UserId), cancellationToken);
    }

    private async Task AddPrIfPresent(Guid userId, PersonalRecordLift lift, decimal? weight)
    {
        if (weight is null) return;
        var pr = PersonalRecord.Create(userId, lift, weight.Value, DateTime.UtcNow);
        await personalRecordRepository.AddAsync(pr);
    }
}
