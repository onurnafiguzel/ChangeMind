namespace ChangeMind.Application.UseCases.PersonalRecords.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;

public class AddPersonalRecordCommandHandler(
    IUserRepository userRepository,
    IPersonalRecordRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddPersonalRecordCommand, Guid>
{
    public async Task<Guid> Handle(AddPersonalRecordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        var pr = PersonalRecord.Create(
            userId:     user.Id,
            lift:       request.Lift,
            weightKg:   request.WeightKg,
            recordedAt: request.RecordedAt ?? DateTime.UtcNow);

        await repository.AddAsync(pr);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return pr.Id;
    }
}
