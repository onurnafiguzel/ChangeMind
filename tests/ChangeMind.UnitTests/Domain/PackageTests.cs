namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using FluentAssertions;

public class PackageTests
{
    // -----------------------------------------------------------------------
    // Create
    // -----------------------------------------------------------------------

    [Fact]
    public void Create_ShouldSetAllFields()
    {
        var package = Package.Create("Premium", "Açıklama", price: 300m, durationDays: 30, PackageType.Premium);

        package.Name.Should().Be("Premium");
        package.Description.Should().Be("Açıklama");
        package.Price.Should().Be(300m);
        package.DurationDays.Should().Be(30);
        package.Type.Should().Be(PackageType.Premium);
    }

    [Fact]
    public void Create_ShouldHaveCorrectDefaults()
    {
        var package = Package.Create("Basic", "Açıklama", 100m, 15, PackageType.Basic);

        package.Id.Should().NotBe(Guid.Empty);
        package.IsActive.Should().BeTrue();
        package.UpdatedAt.Should().BeNull();
    }

    [Theory]
    [InlineData(PackageType.Basic,    100)]
    [InlineData(PackageType.Standard, 200)]
    [InlineData(PackageType.Premium,  400)]
    public void Create_ShouldSetTypeAndPrice(PackageType type, decimal price)
    {
        var package = Package.Create("Test", "Açıklama", price, durationDays: 30, type);

        package.Type.Should().Be(type);
        package.Price.Should().Be(price);
    }

    // -----------------------------------------------------------------------
    // Update
    // -----------------------------------------------------------------------

    [Fact]
    public void Update_ShouldChangeAllFields()
    {
        var package = Package.Create("Eski", "Eski açıklama", 100m, 15, PackageType.Basic);

        package.Update("Yeni", "Yeni açıklama", 250m, 30, PackageType.Premium);

        package.Name.Should().Be("Yeni");
        package.Description.Should().Be("Yeni açıklama");
        package.Price.Should().Be(250m);
        package.DurationDays.Should().Be(30);
        package.Type.Should().Be(PackageType.Premium);
        package.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldNotChangeId()
    {
        var package = Package.Create("Test", "Açıklama", 100m, 15, PackageType.Basic);
        var originalId = package.Id;

        package.Update("Yeni", "Yeni", 200m, 30, PackageType.Standard);

        package.Id.Should().Be(originalId);
    }

    // -----------------------------------------------------------------------
    // Deactivate / Activate
    // -----------------------------------------------------------------------

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var package = Package.Create("Test", "Açıklama", 100m, 15, PackageType.Basic);

        package.Deactivate();

        package.IsActive.Should().BeFalse();
        package.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_AfterDeactivate_ShouldSetIsActiveTrue()
    {
        var package = Package.Create("Test", "Açıklama", 100m, 15, PackageType.Basic);
        package.Deactivate();

        package.Activate();

        package.IsActive.Should().BeTrue();
    }

    // -----------------------------------------------------------------------
    // Fiyat mantığı
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData(0.01)]
    [InlineData(999999.99)]
    public void Create_ShouldAcceptAnyPositivePrice(decimal price)
    {
        var package = Package.Create("Test", "Açıklama", price, 30, PackageType.Basic);

        package.Price.Should().Be(price);
        package.Price.Should().BePositive();
    }
}
