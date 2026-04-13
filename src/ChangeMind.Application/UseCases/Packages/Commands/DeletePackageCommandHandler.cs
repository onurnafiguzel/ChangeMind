namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class DeletePackageCommandHandler(IPackageRepository packageRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePackageCommand, Unit>
{
    public async Task<Unit> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId);
        if (package == null)
            throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        package.Deactivate();

        await packageRepository.UpdateAsync(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
