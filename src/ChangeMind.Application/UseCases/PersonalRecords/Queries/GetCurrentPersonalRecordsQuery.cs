namespace ChangeMind.Application.UseCases.PersonalRecords.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetCurrentPersonalRecordsQuery(Guid UserId) : IRequest<List<PersonalRecordCurrentDto>>;
