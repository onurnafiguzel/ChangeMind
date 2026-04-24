namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;

public class UserTests
{
    // -----------------------------------------------------------------------
    // Test 1 — En temel test kalıbı: Arrange / Act / Assert
    //
    // Arrange : Test için gereken veriler hazırlanır.
    // Act     : Test edilecek kod çalıştırılır.
    // Assert  : Sonuç beklenenle karşılaştırılır.
    // -----------------------------------------------------------------------
    [Fact]
    public void Create_ShouldSetEmailAndIsActiveTrue()
    {
        // Arrange
        var email        = "ali@example.com";
        var passwordHash = "hashed-password";

        // Act
        var user = User.Create(email, passwordHash);

        // Assert
        Assert.Equal(email, user.Email);
        Assert.True(user.IsActive);
    }

    // -----------------------------------------------------------------------
    // Test 2 — Birden fazla Assert: aynı senaryoda birkaç şeyi birlikte kontrol et
    //
    // Create çağrıldığında hangi alanlar varsayılan değerle gelmeli?
    // -----------------------------------------------------------------------
    [Fact]
    public void Create_ShouldHaveDefaultValues()
    {
        // Arrange
        var user = User.Create("test@example.com", "hash");

        // Assert — birden fazla özelliği aynı testte kontrol edebiliriz
        Assert.NotEqual(Guid.Empty, user.Id);   // Id üretilmeli, boş kalmamalı
        Assert.Null(user.Age);                   // Profil henüz tamamlanmadı
        Assert.Null(user.UpdatedAt);             // Hiç güncellenmedi
    }

    // -----------------------------------------------------------------------
    // Test 3 — Davranış testi: bir metod çağrıldığında state nasıl değişmeli?
    //
    // Deactivate() çağrısının gerçekten IsActive = false yaptığını doğrula.
    // -----------------------------------------------------------------------
    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        // Arrange — önce aktif bir kullanıcı oluştur
        var user = User.Create("ali@example.com", "hash");
        Assert.True(user.IsActive); // başlangıç koşulunu doğrula

        // Act
        user.Deactivate();

        // Assert
        Assert.False(user.IsActive);
        Assert.NotNull(user.UpdatedAt); // UpdatedAt doldurulmalı
    }

    // -----------------------------------------------------------------------
    // Test 4 — [Theory] + [InlineData]: aynı testi farklı girdilerle çalıştır
    //
    // [Fact]   → tek bir senaryo, sabit veri
    // [Theory] → aynı senaryonun birden fazla varyasyonu, her biri InlineData ile beslenir
    //
    // Burada: CompleteProfile'dan sonra FirstName ve LastName doğru set ediliyor mu?
    // -----------------------------------------------------------------------
    [Theory]
    [InlineData("Ali",   "Veli")]
    [InlineData("Ayşe",  "Fatma")]
    [InlineData("",      "")]       // boş string de geçerli — domain kısıtlamıyor
    public void CompleteProfile_ShouldSetFirstAndLastName(string firstName, string lastName)
    {
        // Arrange
        var user = User.Create("test@example.com", "hash");

        // Act
        user.CompleteProfile(firstName, lastName);

        // Assert
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName,  user.LastName);
    }
}
