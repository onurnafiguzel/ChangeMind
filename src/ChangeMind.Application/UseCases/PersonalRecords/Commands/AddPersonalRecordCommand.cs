namespace ChangeMind.Application.UseCases.PersonalRecords.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record AddPersonalRecordCommand(
    Guid UserId,
    PersonalRecordLift Lift,
    decimal WeightKg,
    DateTime? RecordedAt) : IRequest<Guid>;
