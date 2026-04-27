namespace ChangeMind.IntegrationTests.Tests;

using System.Net;
using System.Net.Http.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.IntegrationTests.Fixtures;
using ChangeMind.IntegrationTests.Helpers;
using FluentAssertions;

// IClassFixture<T>: Bu test sınıfı için T fixture'ı bir kez oluşturulur ve paylaşılır.
// PostgreSqlFixture → Docker container'ı başlatır (bir kez).
// WebAppFactory    → ASP.NET Core pipeline'ı in-process başlatır (bir kez).
//
// IAsyncLifetime: InitializeAsync → her test sınıfı öncesinde çalışır (migration + respawner).
//                 DisposeAsync    → test sınıfı bittikten sonra çalışır.
public class PackagesControllerTests(PostgreSqlFixture db)
    : IClassFixture<PostgreSqlFixture>, IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebAppFactory _factory = null!;

    public async Task InitializeAsync()
    {
        _factory = new WebAppFactory(db);
        _client  = _factory.CreateClient();

        await DatabaseHelper.MigrateAsync(_factory.Services);
        await DatabaseHelper.InitializeRespawnerAsync(db.ConnectionString);
    }

    public async Task DisposeAsync()
    {
        await DatabaseHelper.ResetAsync(db.ConnectionString);
        await _factory.DisposeAsync();
    }

    // -----------------------------------------------------------------------
    // GET /api/packages
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetPackages_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/packages");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetPackages_EmptyDatabase_ShouldReturnEmptyList()
    {
        var response = await _client.GetAsync("/api/packages");
        var body     = await response.Content.ReadFromJsonAsync<PagedResult<PackageDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Data.Should().BeEmpty();
        body.Total.Should().Be(0);
    }

    // -----------------------------------------------------------------------
    // POST /api/packages
    // -----------------------------------------------------------------------

    [Fact]
    public async Task CreatePackage_ValidRequest_ShouldReturn201()
    {
        var request = new
        {
            Name        = "Premium Plan",
            Description = "Test açıklaması",
            Price       = 299.99m,
            DurationDays = 30,
            Type        = "Premium"
        };

        var response = await _client.PostAsJsonAsync("/api/packages", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreatePackage_ValidRequest_ShouldAppearInList()
    {
        var request = new
        {
            Name        = "Basic Plan",
            Description = "Giriş seviyesi",
            Price       = 99m,
            DurationDays = 15,
            Type        = "Basic"
        };

        await _client.PostAsJsonAsync("/api/packages", request);

        var listResponse = await _client.GetAsync("/api/packages?isActiveOnly=true");
        var body         = await listResponse.Content.ReadFromJsonAsync<PagedResult<PackageDto>>();

        body!.Data.Should().ContainSingle(p => p.Name == "Basic Plan");
    }

    [Fact]
    public async Task CreatePackage_InvalidType_ShouldReturn400()
    {
        var request = new
        {
            Name        = "Test",
            Description = "Test",
            Price       = 100m,
            DurationDays = 30,
            Type        = "GeçersizTür"
        };

        var response = await _client.PostAsJsonAsync("/api/packages", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePackage_MissingName_ShouldReturn400()
    {
        var request = new
        {
            Description = "İsim yok",
            Price       = 100m,
            DurationDays = 30,
            Type        = "Basic"
        };

        var response = await _client.PostAsJsonAsync("/api/packages", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // -----------------------------------------------------------------------
    // GET /api/packages/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetPackageById_ExistingId_ShouldReturn200WithPackage()
    {
        var createRequest = new
        {
            Name        = "Standard Plan",
            Description = "Orta seviye",
            Price       = 199m,
            DurationDays = 30,
            Type        = "Standard"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/packages", createRequest);
        var createdId      = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await _client.GetAsync($"/api/packages/{createdId}");
        var body        = await getResponse.Content.ReadFromJsonAsync<PackageDto>();

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Name.Should().Be("Standard Plan");
        body.Price.Should().Be(199m);
    }

    [Fact]
    public async Task GetPackageById_NonExistingId_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/packages/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // DELETE /api/packages/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task DeletePackage_ExistingId_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/packages", new
        {
            Name        = "Silinecek Plan",
            Description = "Test",
            Price       = 50m,
            DurationDays = 7,
            Type        = "Basic"
        });
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var deleteResponse = await _client.DeleteAsync($"/api/packages/{id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeletePackage_ShouldDisappearFromActiveList()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/packages", new
        {
            Name        = "Soft Deleted Plan",
            Description = "Test",
            Price       = 50m,
            DurationDays = 7,
            Type        = "Basic"
        });
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        await _client.DeleteAsync($"/api/packages/{id}");

        var listResponse = await _client.GetAsync("/api/packages?isActiveOnly=true");
        var body         = await listResponse.Content.ReadFromJsonAsync<PagedResult<PackageDto>>();

        body!.Data.Should().NotContain(p => p.Id == id);
    }
}
