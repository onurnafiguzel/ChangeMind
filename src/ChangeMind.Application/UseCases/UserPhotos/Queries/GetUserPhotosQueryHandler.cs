namespace ChangeMind.Application.UseCases.UserPhotos.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserPhotosQueryHandler(IUserPhotoRepository photoRepository)
    : IRequestHandler<GetUserPhotosQuery, List<UserPhotoDto>>
{
    public async Task<List<UserPhotoDto>> Handle(GetUserPhotosQuery request, CancellationToken cancellationToken)
    {
        var photos = await photoRepository.GetByUserIdAsync(request.UserId);

        // For each ViewType, return the most recently uploaded photo (at most 4 in total).
        return photos
            .GroupBy(p => p.ViewType)
            .Select(g => g.OrderByDescending(p => p.UploadedAt).First())
            .OrderBy(p => p.ViewType)
            .Select(p => new UserPhotoDto
            {
                Id         = p.Id,
                UserId     = p.UserId,
                ImageUrl   = p.ImageUrl,
                ViewType   = p.ViewType,
                UploadedAt = p.UploadedAt
            })
            .ToList();
    }
}
