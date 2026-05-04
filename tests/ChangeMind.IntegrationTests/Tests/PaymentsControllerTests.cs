namespace ChangeMind.IntegrationTests.Tests;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Payments.Commands;
using ChangeMind.Application.UseCases.Payments.Queries;
using ChangeMind.IntegrationTests.Fixtures;
using ChangeMind.IntegrationTests.Helpers;
using FluentAssertions;

public class PaymentsControllerTests(PostgreSqlFixture db)
    : IClassFixture<PostgreSqlFixture>, IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

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

    private async Task<(Guid UserId, Guid PackageId, string Token)> SetupAsync(string email = "pay@example.com")
    {
        var userResp = await _client.PostAsJsonAsync("/api/users", new { Email = email, Password = "Test1234!" });
        userResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var userId = await userResp.Content.ReadFromJsonAsync<Guid>();

        var pkgResp = await _client.PostAsJsonAsync("/api/packages", new
        {
            Name         = "Test Package",
            Description  = "Integration test package",
            Price        = 100m,
            DurationDays = 30,
            Type         = "Basic"
        });
        pkgResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var packageId = await pkgResp.Content.ReadFromJsonAsync<Guid>();

        var authResp = await _client.PostAsJsonAsync("/api/auth/login", new { Email = email, Password = "Test1234!" });
        authResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var auth = await authResp.Content.ReadFromJsonAsync<AuthTokenResponse>();

        return (userId, packageId, auth!.AccessToken);
    }

    // -----------------------------------------------------------------------
    // POST /api/payments
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ProcessPayment_ValidRequest_ShouldReturn200()
    {
        var (userId, packageId, token) = await SetupAsync("paypayment@example.com");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/payments")
        {
            Content = JsonContent.Create(new { UserId = userId, PackageId = packageId, Amount = 100m })
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaymentProcessResponse>();
        result!.Success.Should().BeTrue();
        result.PaymentId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task ProcessPayment_Unauthenticated_ShouldReturn401()
    {
        var response = await _client.PostAsJsonAsync("/api/payments",
            new { UserId = Guid.NewGuid(), PackageId = Guid.NewGuid(), Amount = 50m });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // -----------------------------------------------------------------------
    // GET /api/payments/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetPayment_ExistingId_ShouldReturn200()
    {
        var (userId, packageId, token) = await SetupAsync("getpayment@example.com");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payReq = new HttpRequestMessage(HttpMethod.Post, "/api/payments")
        {
            Content = JsonContent.Create(new { UserId = userId, PackageId = packageId, Amount = 100m })
        };
        payReq.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
        var postResp = await _client.SendAsync(payReq);
        var created = await postResp.Content.ReadFromJsonAsync<PaymentProcessResponse>();

        var response = await _client.GetAsync($"/api/payments/{created!.PaymentId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<PaymentDto>(JsonOptions);
        body!.Id.Should().Be(created.PaymentId);
        body.Amount.Should().Be(100m);
    }

    [Fact]
    public async Task GetPayment_NonExistingId_ShouldReturn404()
    {
        var (_, _, token) = await SetupAsync("getpayment404@example.com");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync($"/api/payments/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
