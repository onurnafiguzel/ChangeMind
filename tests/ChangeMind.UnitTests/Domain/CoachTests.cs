namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using FluentAssertions;

public class CoachTests
{
    // -----------------------------------------------------------------------
    // Create
    // -----------------------------------------------------------------------

    [Fact]
    public void Create_ShouldSetRequiredFields()
    {
        var coach = Coach.Create("coach@example.com", "hash", "Ali", "Veli");

        coach.Email.Should().Be("coach@example.com");
        coach.FirstName.Should().Be("Ali");
        coach.LastName.Should().Be("Veli");
        coach.PasswordHash.Should().Be("hash");
    }

    [Fact]
    public void Create_ShouldHaveCorrectDefaults()
    {
        var coach = Coach.Create("coach@example.com", "hash", "Ali", "Veli");

        coach.Id.Should().NotBe(Guid.Empty);
        coach.IsActive.Should().BeTrue();
        coach.Role.Should().Be(UserRole.Coach);
        coach.UpdatedAt.Should().BeNull();
        coach.Specialization.Should().BeNull();
    }

    [Theory]
    [InlineData(CoachSpecialization.Strength)]
    [InlineData(CoachSpecialization.Cardio)]
    [InlineData(null)]
    public void Create_ShouldSetSpecialization(CoachSpecialization? specialization)
    {
        var coach = Coach.Create("c@example.com", "hash", "Ali", "Veli", specialization);

        coach.Specialization.Should().Be(specialization);
    }

    // -----------------------------------------------------------------------
    // Deactivate / Activate
    // -----------------------------------------------------------------------

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalseAndUpdateUpdatedAt()
    {
        var coach = Coach.Create("c@example.com", "hash", "Ali", "Veli");

        coach.Deactivate();

        coach.IsActive.Should().BeFalse();
        coach.UpdatedAt.Should().NotBeNull();
        coach.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Activate_AfterDeactivate_ShouldSetIsActiveTrue()
    {
        var coach = Coach.Create("c@example.com", "hash", "Ali", "Veli");
        coach.Deactivate();

        coach.Activate();

        coach.IsActive.Should().BeTrue();
    }

    // -----------------------------------------------------------------------
    // ChangePassword
    // -----------------------------------------------------------------------

    [Fact]
    public void ChangePassword_ShouldUpdatePasswordHashAndSetUpdatedAt()
    {
        var coach = Coach.Create("c@example.com", "old-hash", "Ali", "Veli");

        coach.ChangePassword("new-hash");

        coach.PasswordHash.Should().Be("new-hash");
        coach.UpdatedAt.Should().NotBeNull();
    }

    // -----------------------------------------------------------------------
    // Update
    // -----------------------------------------------------------------------

    [Fact]
    public void Update_ShouldChangeNameAndSpecialization()
    {
        var coach = Coach.Create("c@example.com", "hash", "Ali", "Veli");

        coach.Update("Ayşe", "Fatma", CoachSpecialization.Strength);

        coach.FirstName.Should().Be("Ayşe");
        coach.LastName.Should().Be("Fatma");
        coach.Specialization.Should().Be(CoachSpecialization.Strength);
        coach.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldNotChangeEmailOrPasswordHash()
    {
        var coach = Coach.Create("c@example.com", "hash", "Ali", "Veli");

        coach.Update("Yeni", "İsim");

        coach.Email.Should().Be("c@example.com");
        coach.PasswordHash.Should().Be("hash");
    }
}
