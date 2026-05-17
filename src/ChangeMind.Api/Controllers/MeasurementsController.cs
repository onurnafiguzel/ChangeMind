namespace ChangeMind.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.BodyMeasurements.Commands;
using ChangeMind.Application.UseCases.BodyMeasurements.Queries;
using ChangeMind.Domain.Exceptions;

[ApiController]
[Authorize]
[Route("api/users/{userId:guid}/body-measurements")]
public class MeasurementsController(IMediator mediator, ICoachUserRepository coachUserRepository) : ControllerBase
{
    /// <summary>
    /// Add a new body-measurement snapshot. Server computes BodyFatPercent (US Navy) when inputs allow.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> Add(
        Guid userId,
        [FromBody] AddBodyMeasurementRequest body,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);

        var command = new AddBodyMeasurementCommand(
            UserId:      userId,
            RecordedAt:  body.RecordedAt,
            WeightKg:    body.WeightKg,
            WaistCm:     body.WaistCm,
            ArmCm:       body.ArmCm,
            LegCm:       body.LegCm,
            NeckCm:      body.NeckCm,
            HipCm:       body.HipCm,
            Notes:       body.Notes);

        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { userId, measurementId = id }, id);
    }

    /// <summary>List the user's most recent body-measurement snapshots.</summary>
    [HttpGet]
    public async Task<ActionResult<List<BodyMeasurementDto>>> List(
        Guid userId,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);
        var result = await mediator.Send(new GetUserBodyMeasurementsQuery(userId, take), cancellationToken);
        return Ok(result);
    }

    /// <summary>Return the two most recent measurements and per-field deltas.</summary>
    [HttpGet("comparison")]
    public async Task<ActionResult<BodyMeasurementComparisonDto>> Comparison(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);
        var result = await mediator.Send(new GetBodyMeasurementComparisonQuery(userId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{measurementId:guid}")]
    public async Task<ActionResult<BodyMeasurementDto>> GetById(
        Guid userId,
        Guid measurementId,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);
        var result = await mediator.Send(new GetBodyMeasurementByIdQuery(measurementId), cancellationToken);
        if (result is null || result.UserId != userId) return NotFound();
        return Ok(result);
    }

    private async Task AuthorizeUserAccess(Guid userId)
    {
        var role        = User.FindFirstValue("role");
        var tokenUserId = User.FindFirstValue("sub");

        if (role == "Admin" || role == "Coach") return;
        if (Guid.TryParse(tokenUserId, out var tokenGuid))
        {
            if (tokenGuid == userId) return;
            if (role == "Coach" && await coachUserRepository.IsActiveAssignmentAsync(tokenGuid, userId))
                return;
        }
        throw new ForbiddenException("You are not allowed to access this user's measurements.");
    }
}

public record AddBodyMeasurementRequest(
    DateTime? RecordedAt,
    decimal? WeightKg,
    decimal? WaistCm,
    decimal? ArmCm,
    decimal? LegCm,
    decimal? NeckCm,
    decimal? HipCm,
    string? Notes);
