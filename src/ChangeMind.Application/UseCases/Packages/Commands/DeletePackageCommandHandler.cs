namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class DeletePackageCommandHandler(
    IPackageRepository packageRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache)
    : IRequestHandler<DeletePackageCommand, Unit>
{
    public async Task<Unit> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId)
            ?? throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        package.Deactivate();

        await packageRepository.UpdateAsync(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await Task.WhenAll(
            cache.RemoveAsync(CacheKeys.Package(request.PackageId), cancellationToken),
            cache.RemoveByPatternAsync(CacheKeys.PackageListPattern(), cancellationToken));

        return Unit.Value;
    }
}
