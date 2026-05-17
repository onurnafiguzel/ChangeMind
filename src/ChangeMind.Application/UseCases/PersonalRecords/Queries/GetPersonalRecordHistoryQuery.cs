namespace ChangeMind.Application.UseCases.PersonalRecords.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Enums;

public record GetPersonalRecordHistoryQuery(Guid UserId, PersonalRecordLift Lift) : IRequest<List<PersonalRecordDto>>;
