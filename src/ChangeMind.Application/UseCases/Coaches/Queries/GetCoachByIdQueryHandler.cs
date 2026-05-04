namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Exceptions;
using Microsoft.Extensions.Options;

public class GetCoachByIdQueryHandler(
    ICoachRepository coachRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions) : IRequestHandler<GetCoachByIdQuery, CoachDto>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<CoachDto> Handle(GetCoachByIdQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.Coach(request.CoachId);

        var cached = await cache.GetAsync<CoachDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await coachRepository.GetById(request.CoachId)
            .Select(c => new CoachDto
            {
                Id = c.Id,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Specialization = c.Specialization.ToString(),
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        await cache.SetAsync(key, dto, TimeSpan.FromSeconds(_cacheOptions.CoachTtlSeconds), cancellationToken);

        return dto;
    }
}
