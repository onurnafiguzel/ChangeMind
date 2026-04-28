namespace ChangeMind.Application.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string firstName, CancellationToken cancellationToken = default);
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
