namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class GetMuscleGroupsQueryHandler(
    IExerciseRepository exerciseRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions) : IRequestHandler<GetMuscleGroupsQuery, IReadOnlyList<string>>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<IReadOnlyList<string>> Handle(
        GetMuscleGroupsQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.MuscleGroups();

        var cached = await cache.GetAsync<List<string>>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var result = await exerciseRepository
            .GetAll(activeOnly: true)
            .Select(e => e.MuscleGroup.ToString())
            .Distinct()
            .OrderBy(m => m)
            .ToListAsync(cancellationToken);

        await cache.SetAsync(key, result, TimeSpan.FromSeconds(_cacheOptions.ExerciseTtlSeconds), cancellationToken);

        return result;
    }
}
