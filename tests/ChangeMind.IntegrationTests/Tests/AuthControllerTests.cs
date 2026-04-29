namespace ChangeMind.IntegrationTests.Tests;

using System.Net;
using System.Net.Http.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.IntegrationTests.Fixtures;
using ChangeMind.IntegrationTests.Helpers;
using FluentAssertions;

public class AuthControllerTests(PostgreSqlFixture db)
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

    private async Task RegisterUserAsync(string email, string password = "Test1234!")
    {
        var resp = await _client.PostAsJsonAsync("/api/users", new { Email = email, Password = password });
        resp.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    // -----------------------------------------------------------------------
    // POST /api/auth/login
    // -----------------------------------------------------------------------

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturn200WithTokens()
    {
        await RegisterUserAsync("auth.login@example.com");

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Email    = "auth.login@example.com",
            Password = "Test1234!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WrongPassword_ShouldReturn401()
    {
        await RegisterUserAsync("auth.wrongpw@example.com");

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Email    = "auth.wrongpw@example.com",
            Password = "WrongPassword!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_NonExistingEmail_ShouldReturn401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Email    = "nonexistent@example.com",
            Password = "Test1234!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_MissingEmail_ShouldReturn400()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Password = "Test1234!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
