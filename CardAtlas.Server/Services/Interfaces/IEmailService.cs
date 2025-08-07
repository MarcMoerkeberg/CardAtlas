using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Services.Interfaces;

public interface IEmailService
{
	Task<EmailResult> SendEmailAsync(string toEmail, string subject, string body);
	Task<EmailResult> SendEmailAsync(OutboxEmail outboxEmail);
}
