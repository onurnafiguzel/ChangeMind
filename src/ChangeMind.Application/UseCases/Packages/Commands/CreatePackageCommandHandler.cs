namespace ChangeMind.Application.UseCases.Packages.Commands;

using MediatR;
using ChangeMind.Application.Extensions;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class CreatePackageCommandHandler(IPackageRepository packageRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePackageCommand, Guid>
{
    public async Task<Guid> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        var exists = await packageRepository.ExistsAsync(request.Name);
        if (exists)
            throw new ConflictException($"Package with name '{request.Name}' already exists.");

        var packageType = request.Type.ParseOrThrow<PackageType>();

        var package = Package.Create(
            name: request.Name,
            description: request.Description,
            price: request.Price,
            durationDays: request.DurationDays,
            type: packageType);

        await packageRepository.AddAsync(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return package.Id;
    }
}
