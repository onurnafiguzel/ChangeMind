namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using FluentAssertions;

public class WaitingUserTests
{
    // -----------------------------------------------------------------------
    // Create
    // -----------------------------------------------------------------------

    [Fact]
    public void Create_ShouldSetUserId()
    {
        var userId = Guid.NewGuid();

        var waitingUser = WaitingUser.Create(userId);

        waitingUser.UserId.Should().Be(userId);
    }

    [Fact]
    public void Create_ShouldHaveCorrectDefaults()
    {
        var waitingUser = WaitingUser.Create(Guid.NewGuid());

        waitingUser.Id.Should().NotBe(Guid.Empty);
        waitingUser.IsWaitingForAssignment.Should().BeTrue();
        waitingUser.UpdatedAt.Should().BeNull();
    }

    // -----------------------------------------------------------------------
    // MarkAsAssigned
    // -----------------------------------------------------------------------

    [Fact]
    public void MarkAsAssigned_ShouldSetIsWaitingFalseAndUpdateUpdatedAt()
    {
        var waitingUser = WaitingUser.Create(Guid.NewGuid());

        waitingUser.MarkAsAssigned();

        waitingUser.IsWaitingForAssignment.Should().BeFalse();
        waitingUser.UpdatedAt.Should().NotBeNull();
        waitingUser.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }

    // -----------------------------------------------------------------------
    // MarkAsWaiting
    // -----------------------------------------------------------------------

    [Fact]
    public void MarkAsWaiting_AfterAssigned_ShouldSetIsWaitingTrueAgain()
    {
        var waitingUser = WaitingUser.Create(Guid.NewGuid());
        waitingUser.MarkAsAssigned();

        waitingUser.MarkAsWaiting();

        waitingUser.IsWaitingForAssignment.Should().BeTrue();
        waitingUser.UpdatedAt.Should().NotBeNull();
    }

    // -----------------------------------------------------------------------
    // State transitions
    // -----------------------------------------------------------------------

    [Fact]
    public void Transitions_ShouldNotChangeId()
    {
        var waitingUser = WaitingUser.Create(Guid.NewGuid());
        var originalId  = waitingUser.Id;

        waitingUser.MarkAsAssigned();
        waitingUser.MarkAsWaiting();

        waitingUser.Id.Should().Be(originalId);
    }

    [Fact]
    public void Transitions_ShouldNotChangeUserId()
    {
        var userId      = Guid.NewGuid();
        var waitingUser = WaitingUser.Create(userId);

        waitingUser.MarkAsAssigned();
        waitingUser.MarkAsWaiting();

        waitingUser.UserId.Should().Be(userId);
    }
}
