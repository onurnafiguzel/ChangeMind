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
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public IQueryable<User> GetById(Guid id)
    {
        return context.Users.AsNoTracking().Where(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive == true);
    }

    public IQueryable<User> GetAll(bool? isActive = null)
    {
        var query = context.Users.AsNoTracking().AsQueryable();

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
        return context.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .GroupJoin(
                context.FitnessGoals,
                u => u.FitnessGoalId,
                f => f.Id,
                (user, fitnessGoals) => new { user, fitnessGoals = fitnessGoals.DefaultIfEmpty() })
            .SelectMany(
                x => x.fitnessGoals,
                (x, fitnessGoal) => new UserDto
                {
                    Id = x.user.Id,
                    Email = x.user.Email,
                    FirstName = x.user.FirstName,
                    LastName = x.user.LastName,
                    Age = x.user.Age,
                    Height = x.user.Height,
                    Weight = x.user.Weight,
                    Gender = x.user.Gender,
                    FitnessGoal = fitnessGoal == null ? null : fitnessGoal.Description,
                    FitnessLevel = x.user.FitnessLevel,
                    CreatedAt = x.user.CreatedAt,
                });
    }

    public IQueryable<UserDto> GetAllWithFitnessGoal(bool? isActive = null)
    {
        var query = context.Users.AsNoTracking().AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(u => u.IsActive == isActive.Value);
        }

        return query
            .GroupJoin(
                context.FitnessGoals,
                u => u.FitnessGoalId,
                f => f.Id,
                (user, fitnessGoals) => new { user, fitnessGoals = fitnessGoals.DefaultIfEmpty() })
            .SelectMany(
                x => x.fitnessGoals,
                (x, fitnessGoal) => new UserDto
                {
                    Id = x.user.Id,
                    Email = x.user.Email,
                    FirstName = x.user.FirstName,
                    LastName = x.user.LastName,
                    Age = x.user.Age,
                    Height = x.user.Height,
                    Weight = x.user.Weight,
                    Gender = x.user.Gender,
                    FitnessGoal = fitnessGoal == null ? null : fitnessGoal.Description,
                    FitnessLevel = x.user.FitnessLevel,
                    CreatedAt = x.user.CreatedAt,
                });
    }
}
