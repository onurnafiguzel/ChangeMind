namespace ChangeMind.UnitTests.Helpers;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;

/// <summary>
/// Test verisi oluşturmak için merkezi factory — her testte tekrarlanan User/Coach
/// oluşturma kodunu buraya taşıyarak testleri sade tutarız.
/// </summary>
public static class TestUserFactory
{
    public static User CreateUser(
        string email = "test@example.com",
        string passwordHash = "hashed-password",
        string firstName = "Test",
        string lastName = "User") =>
        User.Create(email, passwordHash, firstName, lastName);

    public static User CreateActiveUser(string email = "active@example.com") =>
        User.Create(email, "hash", "Active", "User");

    public static User CreateInactiveUser(string email = "inactive@example.com")
    {
        var user = User.Create(email, "hash", "Inactive", "User");
        user.Deactivate();
        return user;
    }

    public static List<User> CreateUserList(int count = 5)
    {
        return Enumerable.Range(1, count)
            .Select(i => User.Create($"user{i}@example.com", $"hash{i}", $"User{i}", $"Last{i}"))
            .ToList();
    }
}

public static class TestCoachFactory
{
    public static Coach CreateCoach(
        string email = "coach@example.com",
        string passwordHash = "hashed-password",
        string firstName = "Test",
        string lastName = "Coach",
        CoachSpecialization? specialization = CoachSpecialization.Fitness) =>
        Coach.Create(email, passwordHash, firstName, lastName, specialization);
}
