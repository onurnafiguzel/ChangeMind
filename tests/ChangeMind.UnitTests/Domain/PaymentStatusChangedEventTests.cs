namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Events;
using FluentAssertions;

public class PaymentStatusChangedEventTests
{
    [Fact]
    public void MarkAsCompleted_ShouldRaiseDomainEvent()
    {
        // Arrange
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 150m);

        // Act
        payment.MarkAsCompleted("TXN-EVENT");

        // Assert
        payment.DomainEvents.Should().ContainSingle();
        var ev = payment.DomainEvents.Single().Should().BeOfType<PaymentStatusChangedEvent>().Subject;
        ev.OldStatus.Should().Be(PaymentStatus.Pending);
        ev.NewStatus.Should().Be(PaymentStatus.Completed);
        ev.Amount.Should().Be(150m);
        ev.PaymentId.Should().Be(payment.Id);
    }

    [Fact]
    public void MarkAsFailed_ShouldRaiseDomainEvent()
    {
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 100m);
        payment.MarkAsFailed();

        payment.DomainEvents.Should().ContainSingle();
        var ev = payment.DomainEvents.Single().Should().BeOfType<PaymentStatusChangedEvent>().Subject;
        ev.NewStatus.Should().Be(PaymentStatus.Failed);
    }

    [Fact]
    public void MarkAsRefunded_ShouldRaiseDomainEvent()
    {
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 100m);
        payment.MarkAsCompleted("TXN-1");
        payment.ClearDomainEvents();

        payment.MarkAsRefunded();

        payment.DomainEvents.Should().ContainSingle();
        var ev = payment.DomainEvents.Single().Should().BeOfType<PaymentStatusChangedEvent>().Subject;
        ev.OldStatus.Should().Be(PaymentStatus.Completed);
        ev.NewStatus.Should().Be(PaymentStatus.Refunded);
    }

    [Fact]
    public void PaymentStatusChangedEvent_ShouldHaveMetadata()
    {
        var ev = new PaymentStatusChangedEvent(Guid.NewGuid(), Guid.NewGuid(), PaymentStatus.Pending, PaymentStatus.Completed, 200m);

        ev.Id.Should().NotBe(Guid.Empty);
        ev.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
