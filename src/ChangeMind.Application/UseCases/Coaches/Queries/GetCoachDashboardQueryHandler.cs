namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetCoachDashboardQueryHandler(
    ICoachRepository coachRepository,
    ITrainingProgramRepository trainingProgramRepository,
    INutritionPlanRepository nutritionPlanRepository,
    IWaitingUserRepository waitingUserRepository,
    IUserRepository userRepository)
    : IRequestHandler<GetCoachDashboardQuery, CoachDashboardDto>
{
    public async Task<CoachDashboardDto> Handle(GetCoachDashboardQuery request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId)
            ?? throw new NotFoundException($"Coach '{request.CoachId}' not found.");

        var activePrograms = await trainingProgramRepository.GetActiveByCoachIdAsync(request.CoachId);
        var assignedUserIds = activePrograms.Select(p => p.UserId).Distinct().ToList();

        var pendingWaiting = await waitingUserRepository
            .GetWaitingForAssignment()
            .CountAsync(cancellationToken);

        var recentPrograms = activePrograms
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
            .Select(p => new CoachProgramListItemDto
            {
                Id                 = p.Id,
                Name               = p.Name,
                Description        = p.Description,
                DurationWeeks      = p.DurationWeeks,
                CompletedWeeks     = p.CompletedWeeks,
                ProgressPercentage = p.ProgressPercentage,
                Difficulty         = p.Difficulty,
                StartDate          = p.StartDate,
                EndDate            = p.EndDate,
                CreatedAt          = p.CreatedAt,
                IsCompleted        = p.IsCompleted,
                UserId             = p.UserId,
                UserAge            = p.AssignedTo?.Age,
                UserHeight         = p.AssignedTo?.Height,
                UserWeight         = p.AssignedTo?.Weight,
                UserGender         = p.AssignedTo?.Gender
            })
            .ToList();

        // Build assigned users breakdown — fetch waiting flags per user (gives HasTrainingProgram/HasNutritionPlan)
        var assignedUsers = new List<CoachDashboardAssignedUserDto>();
        foreach (var program in activePrograms.GroupBy(p => p.UserId).Select(g => g.First()))
        {
            var waiting = await waitingUserRepository.GetByUserIdAsync(program.UserId);
            var hasNutrition = waiting?.HasNutritionPlan
                ?? (await nutritionPlanRepository.GetLatestActiveByUserIdAsync(program.UserId)) is not null;

            assignedUsers.Add(new CoachDashboardAssignedUserDto
            {
                UserId             = program.UserId,
                FullName           = program.AssignedTo != null
                    ? $"{program.AssignedTo.FirstName} {program.AssignedTo.LastName}"
                    : string.Empty,
                HasTrainingProgram = waiting?.HasTrainingProgram ?? true,
                HasNutritionPlan   = hasNutrition
            });
        }

        return new CoachDashboardDto
        {
            Coach = new CoachSummaryDto
            {
                Id             = coach.Id,
                FirstName      = coach.FirstName,
                LastName       = coach.LastName,
                Specialization = coach.Specialization
            },
            AssignedUserCount       = assignedUserIds.Count,
            ActiveProgramCount      = activePrograms.Count,
            PendingWaitingUserCount = pendingWaiting,
            RecentPrograms          = recentPrograms,
            AssignedUsers           = assignedUsers
        };
    }
}
