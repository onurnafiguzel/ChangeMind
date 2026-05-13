namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class ActivateNutritionPlanCommandHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivateNutritionPlanCommand>
{
    public async Task Handle(ActivateNutritionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await nutritionPlanRepository.GetByIdAsync(request.PlanId)
            ?? throw new NotFoundException($"Nutrition plan '{request.PlanId}' not found.");

        // Deactivate other active plans of this user (single-active rule)
        var others = await nutritionPlanRepository.GetActiveByUserIdAsync(plan.UserId);
        foreach (var other in others.Where(o => o.Id != plan.Id))
        {
            other.Deactivate();
            await nutritionPlanRepository.UpdateAsync(other);
        }

        plan.Activate();
        await nutritionPlanRepository.UpdateAsync(plan);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public class DeactivateNutritionPlanCommandHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeactivateNutritionPlanCommand>
{
    public async Task Handle(DeactivateNutritionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await nutritionPlanRepository.GetByIdAsync(request.PlanId)
            ?? throw new NotFoundException($"Nutrition plan '{request.PlanId}' not found.");

        plan.Deactivate();
        await nutritionPlanRepository.UpdateAsync(plan);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
