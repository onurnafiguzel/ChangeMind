namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using FluentAssertions;

public class ExerciseTests
{
    // -----------------------------------------------------------------------
    // Create
    // -----------------------------------------------------------------------

    [Fact]
    public void Create_ShouldSetRequiredFields()
    {
        var exercise = Exercise.Create("Squat", MuscleGroup.Legs, DifficultyLevel.Intermediate);

        exercise.MovementName.Should().Be("Squat");
        exercise.MuscleGroup.Should().Be(MuscleGroup.Legs);
        exercise.DifficultyLevel.Should().Be(DifficultyLevel.Intermediate);
    }

    [Fact]
    public void Create_ShouldHaveCorrectDefaults()
    {
        var exercise = Exercise.Create("Squat", MuscleGroup.Legs, DifficultyLevel.Beginner);

        exercise.Id.Should().NotBe(Guid.Empty);
        exercise.IsActive.Should().BeTrue();
        exercise.UpdatedAt.Should().BeNull();
        exercise.Description.Should().BeNull();
        exercise.VideoLink.Should().BeNull();
    }

    [Fact]
    public void Create_WithOptionalFields_ShouldSetThem()
    {
        var exercise = Exercise.Create(
            "Bench Press",
            MuscleGroup.Chest,
            DifficultyLevel.Advanced,
            description: "Göğüs egzersizi",
            videoLink: "https://example.com/video");

        exercise.Description.Should().Be("Göğüs egzersizi");
        exercise.VideoLink.Should().Be("https://example.com/video");
    }

    [Theory]
    [InlineData(DifficultyLevel.Beginner)]
    [InlineData(DifficultyLevel.Intermediate)]
    [InlineData(DifficultyLevel.Advanced)]
    public void Create_ShouldAcceptAllDifficultyLevels(DifficultyLevel level)
    {
        var exercise = Exercise.Create("Test", MuscleGroup.Back, level);

        exercise.DifficultyLevel.Should().Be(level);
    }

    // -----------------------------------------------------------------------
    // Update
    // -----------------------------------------------------------------------

    [Fact]
    public void Update_ShouldChangeAllFields()
    {
        var exercise = Exercise.Create("Squat", MuscleGroup.Legs, DifficultyLevel.Beginner);

        exercise.Update("Deadlift", MuscleGroup.Back, DifficultyLevel.Advanced, "Sırt egzersizi", null);

        exercise.MovementName.Should().Be("Deadlift");
        exercise.MuscleGroup.Should().Be(MuscleGroup.Back);
        exercise.DifficultyLevel.Should().Be(DifficultyLevel.Advanced);
        exercise.Description.Should().Be("Sırt egzersizi");
        exercise.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldClearVideoLinkWhenNull()
    {
        var exercise = Exercise.Create("Squat", MuscleGroup.Legs, DifficultyLevel.Beginner,
            videoLink: "https://example.com/video");

        exercise.Update("Squat", MuscleGroup.Legs, DifficultyLevel.Beginner, null, videoLink: null);

        exercise.VideoLink.Should().BeNull();
    }

    // -----------------------------------------------------------------------
    // Deactivate / Activate
    // -----------------------------------------------------------------------

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var exercise = Exercise.Create("Squat", MuscleGroup.Legs, DifficultyLevel.Beginner);

        exercise.Deactivate();

        exercise.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_AfterDeactivate_ShouldSetIsActiveTrue()
    {
        var exercise = Exercise.Create("Squat", MuscleGroup.Legs, DifficultyLevel.Beginner);
        exercise.Deactivate();

        exercise.Activate();

        exercise.IsActive.Should().BeTrue();
    }
}
