namespace ChangeMind.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.NutritionPlans.Commands;
using ChangeMind.Application.UseCases.NutritionPlans.Queries;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

[ApiController]
[Route("api/nutrition-plans")]
public class NutritionPlansController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get a nutrition plan by ID. Users may only access their own plans; Coach/Admin can access any.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NutritionPlanDetailDto>> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetNutritionPlanByIdQuery(id), cancellationToken);
        if (result == null) return NotFound();

        AuthorizeViewPlan(result.UserId);
        return Ok(result);
    }

    /// <summary>
    /// Create a new nutrition plan. Coach or Admin only.
    /// Previously active plans for the user are automatically deactivated.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateNutritionPlanRequest body,
        CancellationToken cancellationToken = default)
    {
        var coachId = ResolveCoachIdForWrite();

        var command = new CreateNutritionPlanCommand(
            CoachId:     coachId,
            UserId:      body.UserId,
            Title:       body.Title,
            Description: body.Description,
            Days:        body.Days);

        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Update an existing nutrition plan (title, description, daily plan). Coach or Admin only.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateNutritionPlanRequest body,
        CancellationToken cancellationToken = default)
    {
        EnsureRoleCoachOrAdmin();

        var command = new UpdateNutritionPlanCommand(
            PlanId:      id,
            Title:       body.Title,
            Description: body.Description,
            Days:        body.Days);

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken = default)
    {
        EnsureRoleCoachOrAdmin();
        await mediator.Send(new ActivateNutritionPlanCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken = default)
    {
        EnsureRoleCoachOrAdmin();
        await mediator.Send(new DeactivateNutritionPlanCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get the user's latest active nutrition plan.
    /// User can only request their own; Coach/Admin can request anyone.
    /// </summary>
    [HttpGet("~/api/users/{userId:guid}/active-nutrition-plan")]
    public async Task<ActionResult<NutritionPlanDetailDto>> GetUserActiveNutritionPlan(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        AuthorizeViewPlan(userId);

        var result = await mediator.Send(new GetUserActiveNutritionPlanQuery(userId), cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Export the user's active nutrition plan as an .xlsx file.
    /// Each NutritionDayType gets its own sheet; meals are stacked vertically.
    /// </summary>
    [HttpGet("~/api/users/{userId:guid}/active-nutrition-plan/export")]
    public async Task<IActionResult> ExportUserActiveNutritionPlan(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        AuthorizeViewPlan(userId);

        var result = await mediator.Send(new ExportUserNutritionPlanQuery(userId), cancellationToken);
        if (result is null) return NotFound();

        return File(
            result.Content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            result.FileName);
    }

    /// <summary>
    /// Get all nutrition plans (active + historical) for a user. Coach or Admin only.
    /// </summary>
    [HttpGet("~/api/users/{userId:guid}/nutrition-plans")]
    public async Task<ActionResult<List<NutritionPlanListItemDto>>> GetUserNutritionPlans(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        EnsureRoleCoachOrAdmin();
        var result = await mediator.Send(new GetUserNutritionPlansQuery(userId), cancellationToken);
        return Ok(result);
    }

    // -- helpers --

    private void AuthorizeViewPlan(Guid planUserId)
    {
        var role        = User.FindFirstValue("role");
        var tokenUserId = User.FindFirstValue("sub");

        if (role == "Admin" || role == "Coach") return;

        if (!Guid.TryParse(tokenUserId, out var tokenGuid) || tokenGuid != planUserId)
            throw new ForbiddenException("Users can only view their own nutrition plan.");
    }

    private void EnsureRoleCoachOrAdmin()
    {
        var role = User.FindFirstValue("role");
        if (role != "Admin" && role != "Coach")
            throw new ForbiddenException("Only coaches and admins can perform this action.");
    }

    private Guid ResolveCoachIdForWrite()
    {
        var role = User.FindFirstValue("role");
        if (role != "Admin" && role != "Coach")
            throw new ForbiddenException("Only coaches and admins can create nutrition plans.");

        var tokenUserId = User.FindFirstValue("sub");
        if (!Guid.TryParse(tokenUserId, out var tokenGuid))
            throw new ForbiddenException("Invalid token subject.");

        return tokenGuid;
    }
}

public record CreateNutritionPlanRequest(
    Guid UserId,
    string Title,
    string? Description,
    Dictionary<NutritionDayType, List<MealInput>> Days);

public record UpdateNutritionPlanRequest(
    string Title,
    string? Description,
    Dictionary<NutritionDayType, List<MealInput>> Days);
