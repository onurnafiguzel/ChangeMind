namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;
using ChangeMind.Application.Extensions;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class UpdatePackageCommandHandler(IPackageRepository packageRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePackageCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
    {
        var package = await packageRepository.GetByIdAsync(request.PackageId);
        if (package == null)
            throw new NotFoundException($"Package with ID '{request.PackageId}' not found.");

        if (!package.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await packageRepository.ExistsAsync(request.Name);
            if (exists)
                throw new ConflictException($"Package with name '{request.Name}' already exists.");
        }

        var packageType = request.Type.ParseOrThrow<PackageType>();

        package.Update(
            name: request.Name,
            description: request.Description,
            price: request.Price,
            durationDays: request.DurationDays,
            type: packageType);

        await packageRepository.UpdateAsync(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
