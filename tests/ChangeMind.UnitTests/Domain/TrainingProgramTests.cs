namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using FluentAssertions;

public class TrainingProgramTests
{
    private static TrainingProgram CreateProgram(int durationWeeks = 8) =>
        TrainingProgram.Create(
            name: "Güç Programı",
            description: "Test programı",
            durationWeeks: durationWeeks,
            difficulty: DifficultyLevel.Intermediate,
            coachId: Guid.NewGuid(),
            userId: Guid.NewGuid());

    // -----------------------------------------------------------------------
    // Create
    // -----------------------------------------------------------------------

    [Fact]
    public void Create_ShouldSetPropertiesCorrectly()
    {
        var program = CreateProgram(durationWeeks: 12);

        program.Name.Should().Be("Güç Programı");
        program.DurationWeeks.Should().Be(12);
        program.IsActive.Should().BeTrue();
        program.IsCompleted.Should().BeFalse();
        program.VersionNumber.Should().Be(1);
        program.CompletedWeeks.Should().Be(0);
        program.ProgressPercentage.Should().Be(0);
        program.CompletedAt.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        var act = () => TrainingProgram.Create(
            name: "   ",
            description: null,
            durationWeeks: 8,
            difficulty: null,
            coachId: Guid.NewGuid(),
            userId: Guid.NewGuid());

        act.Should().Throw<ValidationException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(53)]
    public void Create_WithInvalidDuration_ShouldThrowValidationException(int weeks)
    {
        var act = () => TrainingProgram.Create(
            name: "Test",
            description: null,
            durationWeeks: weeks,
            difficulty: null,
            coachId: Guid.NewGuid(),
            userId: Guid.NewGuid());

        act.Should().Throw<ValidationException>();
    }

    // -----------------------------------------------------------------------
    // UpdateDailyProgram
    // -----------------------------------------------------------------------

    [Fact]
    public void UpdateDailyProgram_ShouldIncrementVersionNumber()
    {
        var program = CreateProgram();

        program.UpdateDailyProgram("{\"Pazartesi\":[]}");

        program.VersionNumber.Should().Be(2);
        program.DailyProgramJson.Should().Be("{\"Pazartesi\":[]}");
        program.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateDailyProgram_CalledTwice_ShouldIncrementVersionTwice()
    {
        var program = CreateProgram();

        program.UpdateDailyProgram("{\"Pazartesi\":[]}");
        program.UpdateDailyProgram("{\"Salı\":[]}");

        program.VersionNumber.Should().Be(3);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDailyProgram_WithEmptyJson_ShouldThrowValidationException(string json)
    {
        var program = CreateProgram();

        var act = () => program.UpdateDailyProgram(json);

        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void UpdateDailyProgram_OnInactiveProgram_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Deactivate();

        var act = () => program.UpdateDailyProgram("{\"Pazartesi\":[]}");

        act.Should().Throw<InvalidStateTransitionException>();
    }

    [Fact]
    public void UpdateDailyProgram_OnCompletedProgram_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Complete();

        var act = () => program.UpdateDailyProgram("{\"Pazartesi\":[]}");

        act.Should().Throw<InvalidStateTransitionException>();
    }

    // -----------------------------------------------------------------------
    // Activate / Deactivate
    // -----------------------------------------------------------------------

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var program = CreateProgram();

        program.Deactivate();

        program.IsActive.Should().BeFalse();
        program.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Deactivate_AlreadyInactive_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Deactivate();

        var act = () => program.Deactivate();

        act.Should().Throw<InvalidStateTransitionException>();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrue()
    {
        var program = CreateProgram();
        program.Deactivate();

        program.Activate();

        program.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_OnCompletedProgram_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Complete();

        var act = () => program.Activate();

        act.Should().Throw<InvalidStateTransitionException>();
    }

    // -----------------------------------------------------------------------
    // UpdateProgress
    // -----------------------------------------------------------------------

    [Fact]
    public void UpdateProgress_ShouldSetCompletedWeeks()
    {
        var program = CreateProgram(durationWeeks: 8);

        program.UpdateProgress(4);

        program.CompletedWeeks.Should().Be(4);
        program.ProgressPercentage.Should().Be(50.0);
    }

    [Fact]
    public void UpdateProgress_WithNegativeValue_ShouldThrowValidationException()
    {
        var program = CreateProgram();

        var act = () => program.UpdateProgress(-1);

        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void UpdateProgress_ExceedingDuration_ShouldThrowValidationException()
    {
        var program = CreateProgram(durationWeeks: 8);

        var act = () => program.UpdateProgress(9);

        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void UpdateProgress_OnInactiveProgram_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Deactivate();

        var act = () => program.UpdateProgress(2);

        act.Should().Throw<InvalidStateTransitionException>();
    }

    // -----------------------------------------------------------------------
    // Complete
    // -----------------------------------------------------------------------

    [Fact]
    public void Complete_ShouldSetIsCompletedAndSetCompletedWeeks()
    {
        var program = CreateProgram(durationWeeks: 8);

        program.Complete();

        program.IsCompleted.Should().BeTrue();
        program.IsActive.Should().BeFalse();
        program.CompletedWeeks.Should().Be(8);
        program.CompletedAt.Should().NotBeNull();
        program.ProgressPercentage.Should().Be(100.0);
    }

    [Fact]
    public void Complete_AlreadyCompleted_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Complete();

        var act = () => program.Complete();

        act.Should().Throw<InvalidStateTransitionException>();
    }

    [Fact]
    public void Complete_OnInactiveProgram_ShouldThrowInvalidStateTransition()
    {
        var program = CreateProgram();
        program.Deactivate();

        var act = () => program.Complete();

        act.Should().Throw<InvalidStateTransitionException>();
    }

    // -----------------------------------------------------------------------
    // ProgressPercentage
    // -----------------------------------------------------------------------

    [Fact]
    public void ProgressPercentage_ShouldNeverExceed100()
    {
        // Arrange — edge case: DurationWeeks=4, CompletedWeeks=4 (100%)
        var program = CreateProgram(durationWeeks: 4);
        program.UpdateProgress(4);

        program.ProgressPercentage.Should().Be(100.0);
    }
}
