namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;


public class UserRepository(ChangeMindDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public IQueryable<User> GetById(Guid id)
    {
        return context.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .Where(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive == true);
    }

    public IQueryable<User> GetAll(bool? isActive = null)
    {
        var query = context.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(u => u.IsActive == isActive.Value);
        }

        return query;
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public IQueryable<UserDto> GetUserByIdWithFitnessGoal(Guid id)
    {
        return BuildUserDtoQuery(context.Users.AsNoTracking().Where(u => u.Id == id));
    }

    public IQueryable<UserDto> GetAllWithFitnessGoal(bool? isActive = null)
    {
        var query = context.Users.AsNoTracking().AsQueryable();
        if (isActive.HasValue)
        {
            query = query.Where(u => u.IsActive == isActive.Value);
        }
        return BuildUserDtoQuery(query);
    }

    private IQueryable<UserDto> BuildUserDtoQuery(IQueryable<User> users)
    {
        return users
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.IsCompletedProfile,
                u.IsActive,
                u.CreatedAt,
                Profile = u.Profile
            })
            .GroupJoin(
                context.FitnessGoals,
                x => x.Profile != null ? x.Profile.FitnessGoalId : null,
                f => f.Id,
                (x, fitnessGoals) => new { x, fitnessGoals = fitnessGoals.DefaultIfEmpty() })
            .SelectMany(
                row => row.fitnessGoals,
                (row, fitnessGoal) => new UserDto
                {
                    Id = row.x.Id,
                    Email = row.x.Email,
                    FirstName = row.x.Profile == null ? string.Empty : row.x.Profile.FirstName,
                    LastName = row.x.Profile == null ? string.Empty : row.x.Profile.LastName,
                    Age = row.x.Profile == null ? null : row.x.Profile.Age,
                    Height = row.x.Profile == null ? null : row.x.Profile.Height,
                    Weight = row.x.Profile == null ? null : row.x.Profile.Weight,
                    Gender = row.x.Profile == null ? null : row.x.Profile.Gender,
                    FitnessGoal = fitnessGoal == null ? string.Empty : (fitnessGoal.Name ?? string.Empty),
                    FitnessLevel = row.x.Profile == null ? null : row.x.Profile.FitnessLevel,
                    IsCompletedProfile = row.x.IsCompletedProfile,
                    IsActive = row.x.IsActive,
                    CreatedAt = row.x.CreatedAt,
                    HealthProfile = row.x.Profile == null ? null : new UserHealthBlockDto
                    {
                        DailyWorkLifestyle     = row.x.Profile.DailyWorkLifestyle,
                        GymDaysPerWeek         = row.x.Profile.GymDaysPerWeek,
                        HealthConditions       = row.x.Profile.HealthConditions,
                        FoodAllergies          = row.x.Profile.FoodAllergies,
                        SupplementInterest     = row.x.Profile.SupplementInterest,
                        WantsSupplementSupport = row.x.Profile.WantsSupplementSupport
                    }
                });
    }
}
