namespace ChangeMind.Infrastructure.Services;

using ChangeMind.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public sealed class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendWelcomeEmailAsync(string email, string firstName, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}
