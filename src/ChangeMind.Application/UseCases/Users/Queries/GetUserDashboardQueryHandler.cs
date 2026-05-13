namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.NutritionPlans.Queries;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;
using ChangeMind.Domain.Exceptions;

public class GetUserDashboardQueryHandler(
    IUserRepository userRepository,
    IWaitingUserRepository waitingUserRepository,
    IPaymentRepository paymentRepository,
    IMediator mediator)
    : IRequestHandler<GetUserDashboardQuery, UserDashboardDto>
{
    public async Task<UserDashboardDto> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User '{request.UserId}' not found.");

        // Sequential — DbContext is not thread-safe; these handlers share the same scoped DbContext.
        var activeProgram = await mediator.Send(new GetUserActiveProgramQuery { UserId = request.UserId }, cancellationToken);
        var activeNutrition = await mediator.Send(new GetUserActiveNutritionPlanQuery(request.UserId), cancellationToken);
        var waiting = await waitingUserRepository.GetByUserIdAsync(request.UserId);
        var activePackagePayment = await paymentRepository.GetActivePackageByUserIdAsync(request.UserId);

        WaitingUserStatusDto? waitingStatus = waiting is null ? null : new WaitingUserStatusDto
        {
            IsWaitingForAssignment = waiting.IsWaitingForAssignment,
            HasTrainingProgram     = waiting.HasTrainingProgram,
            HasNutritionPlan       = waiting.HasNutritionPlan,
            CreatedAt              = waiting.CreatedAt
        };

        return new UserDashboardDto
        {
            Profile = new UserDashboardProfileDto
            {
                Id                 = user.Id,
                Email              = user.Email,
                FirstName          = user.FirstName,
                LastName           = user.LastName,
                Age                = user.Age,
                Height             = user.Height,
                Weight             = user.Weight,
                Gender             = user.Gender?.ToString(),
                FitnessLevel       = user.FitnessLevel?.ToString(),
                IsCompletedProfile = user.IsCompletedProfile
            },
            ActiveTrainingProgram = activeProgram,
            ActiveNutritionPlan   = activeNutrition,
            WaitingStatus         = waitingStatus,
            PackageProgress       = BuildPackageProgress(activePackagePayment)
        };
    }

    private static PackageProgressDto? BuildPackageProgress(Domain.Entities.Payment? payment)
    {
        if (payment is null || payment.PackageStartDate is null || payment.PackageEndDate is null)
            return null;

        var now      = DateTime.UtcNow;
        var start    = payment.PackageStartDate.Value;
        var end      = payment.PackageEndDate.Value;
        var totalDays   = Math.Max(1, (int)Math.Round((end - start).TotalDays));
        var elapsedDays = Math.Clamp((int)Math.Floor((now - start).TotalDays), 0, totalDays);
        var remaining   = Math.Max(0, totalDays - elapsedDays);

        var totalSeconds   = (end - start).TotalSeconds;
        var elapsedSeconds = Math.Clamp((now - start).TotalSeconds, 0, totalSeconds);
        var pct            = totalSeconds <= 0 ? 0m : Math.Round((decimal)(elapsedSeconds / totalSeconds) * 100m, 2);

        return new PackageProgressDto
        {
            PaymentId          = payment.Id,
            PackageId          = payment.PackageId,
            PackageName        = payment.Package?.Name ?? string.Empty,
            StartDate          = start,
            EndDate            = end,
            TotalDays          = totalDays,
            ElapsedDays        = elapsedDays,
            RemainingDays      = remaining,
            ProgressPercentage = pct,
            IsExpired          = now >= end
        };
    }
}
