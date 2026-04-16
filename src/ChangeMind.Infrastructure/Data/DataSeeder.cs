namespace ChangeMind.Infrastructure.Data;

using System.Security.Cryptography;
using System.Text;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Seeds development data: admin user, sample coaches, and test users.
/// Idempotent — checks for existing records before inserting.
/// Exercises and packages are seeded via EF Core migrations.
/// </summary>
public class DataSeeder(ChangeMindDbContext context, ILogger<DataSeeder> logger)
{
    // Static GUIDs — stable across runs for idempotency
    private static readonly Guid AdminId   = new("A0000000-0000-0000-0000-000000000001");

    private static readonly Guid[] CoachIds =
    [
        new("C0000000-0000-0000-0000-000000000001"),
        new("C0000000-0000-0000-0000-000000000002"),
        new("C0000000-0000-0000-0000-000000000003"),
        new("C0000000-0000-0000-0000-000000000004"),
        new("C0000000-0000-0000-0000-000000000005"),
        new("C0000000-0000-0000-0000-000000000006"),
    ];

    private static readonly Guid[] UserIds =
    [
        new("U0000000-0000-0000-0000-000000000001"),
        new("U0000000-0000-0000-0000-000000000002"),
        new("U0000000-0000-0000-0000-000000000003"),
        new("U0000000-0000-0000-0000-000000000004"),
        new("U0000000-0000-0000-0000-000000000005"),
    ];

    public async Task SeedAsync()
    {
        await SeedAdminAsync();
        await SeedCoachesAsync();
        await SeedUsersAsync();
        await context.SaveChangesAsync();
    }

    private async Task SeedAdminAsync()
    {
        if (await context.Users.AnyAsync(u => u.Id == AdminId))
            return;

        var admin = User.Seed(
            id:           AdminId,
            email:        "admin@changemind.com",
            passwordHash: Hash("Admin@123!"),
            firstName:    "Admin",
            lastName:     "ChangeMind",
            role:         UserRole.Admin);

        context.Users.Add(admin);
        logger.LogInformation("Seeded admin user: {Email}", admin.Email);
    }

    private async Task SeedCoachesAsync()
    {
        var coaches = new[]
        {
            (CoachIds[0], "coach.ali@changemind.com",     "Ali",     "Yılmaz",  CoachSpecialization.Strength),
            (CoachIds[1], "coach.ayse@changemind.com",    "Ayşe",    "Kaya",    CoachSpecialization.Cardio),
            (CoachIds[2], "coach.mehmet@changemind.com",  "Mehmet",  "Demir",   CoachSpecialization.Bodybuilding),
            (CoachIds[3], "coach.fatma@changemind.com",   "Fatma",   "Çelik",   CoachSpecialization.Yoga),
            (CoachIds[4], "coach.emre@changemind.com",    "Emre",    "Şahin",   CoachSpecialization.CrossFit),
            (CoachIds[5], "coach.zeynep@changemind.com",  "Zeynep",  "Arslan",  CoachSpecialization.Rehabilitation),
        };

        var existingIds = await context.Coaches
            .Where(c => CoachIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        foreach (var (id, email, firstName, lastName, specialization) in coaches)
        {
            if (existingIds.Contains(id))
                continue;

            var coach = Coach.Seed(
                id:             id,
                email:          email,
                passwordHash:   Hash("Coach@123!"),
                firstName:      firstName,
                lastName:       lastName,
                specialization: specialization);

            context.Coaches.Add(coach);
            logger.LogInformation("Seeded coach: {Email}", email);
        }
    }

    private async Task SeedUsersAsync()
    {
        var users = new[]
        {
            (UserIds[0], "user.can@test.com",    "Can",    "Öztürk",  FitnessGoal.MuscleGain,    DifficultyLevel.Beginner),
            (UserIds[1], "user.selin@test.com",  "Selin",  "Koç",     FitnessGoal.FatLoss,       DifficultyLevel.Intermediate),
            (UserIds[2], "user.burak@test.com",  "Burak",  "Kurt",    FitnessGoal.Strength,      DifficultyLevel.Advanced),
            (UserIds[3], "user.elif@test.com",   "Elif",   "Aydın",   FitnessGoal.GeneralFitness,DifficultyLevel.Beginner),
            (UserIds[4], "user.berk@test.com",   "Berk",   "Güneş",   FitnessGoal.Endurance,     DifficultyLevel.Intermediate),
        };

        var existingIds = await context.Users
            .Where(u => UserIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync();

        foreach (var (id, email, firstName, lastName, fitnessGoal, fitnessLevel) in users)
        {
            if (existingIds.Contains(id))
                continue;

            var user = User.Seed(
                id:           id,
                email:        email,
                passwordHash: Hash("User@123!"),
                firstName:    firstName,
                lastName:     lastName,
                role:         UserRole.User);

            user.CompleteProfile(
                firstName:    firstName,
                lastName:     lastName,
                age:          25,
                height:       175,
                weight:       75,
                gender:       Gender.Male,
                fitnessGoal:  fitnessGoal,
                fitnessLevel: fitnessLevel);

            context.Users.Add(user);
            logger.LogInformation("Seeded test user: {Email}", email);
        }
    }

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
