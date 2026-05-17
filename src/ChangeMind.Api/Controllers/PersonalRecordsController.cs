namespace ChangeMind.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.PersonalRecords.Commands;
using ChangeMind.Application.UseCases.PersonalRecords.Queries;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

[ApiController]
[Authorize]
[Route("api/users/{userId:guid}/personal-records")]
public class PersonalRecordsController(IMediator mediator, ICoachUserRepository coachUserRepository) : ControllerBase
{
    /// <summary>
    /// Add a new personal-record entry. Multiple entries per lift are allowed; "current" is the heaviest.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> Add(
        Guid userId,
        [FromBody] AddPersonalRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);

        var command = new AddPersonalRecordCommand(
            UserId:     userId,
            Lift:       body.Lift,
            WeightKg:   body.WeightKg,
            RecordedAt: body.RecordedAt,
            Notes:      body.Notes);

        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Current), new { userId }, id);
    }

    /// <summary>Get the current (heaviest) record for every tracked lift.</summary>
    [HttpGet]
    public async Task<ActionResult<List<PersonalRecordCurrentDto>>> Current(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);
        var result = await mediator.Send(new GetCurrentPersonalRecordsQuery(userId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Full history for a specific lift, newest first.</summary>
    [HttpGet("history")]
    public async Task<ActionResult<List<PersonalRecordDto>>> History(
        Guid userId,
        [FromQuery] PersonalRecordLift lift,
        CancellationToken cancellationToken = default)
    {
        await AuthorizeUserAccess(userId);
        var result = await mediator.Send(new GetPersonalRecordHistoryQuery(userId, lift), cancellationToken);
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
        throw new ForbiddenException("You are not allowed to access this user's personal records.");
    }
}

public record AddPersonalRecordRequest(
    PersonalRecordLift Lift,
    decimal WeightKg,
    DateTime? RecordedAt,
    string? Notes);
