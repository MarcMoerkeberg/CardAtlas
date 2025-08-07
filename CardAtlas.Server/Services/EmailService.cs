using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CardAtlas.Server.Services;

public class EmailService : IEmailService
{
	private readonly SmtpSettings _smtpSettings;
	private readonly SmtpClient _smtpClient;
	private readonly MailboxAddress _fromMailbox;

	public EmailService(IOptions<AppSettings> appSettings, SmtpClient smtpClient)
	{
		_smtpSettings = appSettings.Value.SmtpSettings;
		_smtpClient = smtpClient;
		_fromMailbox = new MailboxAddress(_smtpSettings.FromName, _smtpSettings.From);
	}

	public async Task<EmailResult> SendEmailAsync(OutboxEmail outboxEmail)
	{
		return await SendEmailAsync(outboxEmail.ToEmail, outboxEmail.Subject, outboxEmail.Body);
	}

	public async Task<EmailResult> SendEmailAsync(string toEmail, string subject, string body)
	{
		try
		{
			MimeMessage emailMessage = new();
			BodyBuilder bodyBuilder = new BodyBuilder { HtmlBody = body };

			emailMessage.From.Add(_fromMailbox);
			emailMessage.To.Add(MailboxAddress.Parse(toEmail));
			emailMessage.Subject = subject;
			emailMessage.Body = bodyBuilder.ToMessageBody();

			await _smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port);
			await _smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
			await _smtpClient.SendAsync(emailMessage);
			await _smtpClient.DisconnectAsync(true);

			return EmailResult.Success;
		}
		catch (Exception exception)
		{
			//TODO: Add logging

			return EmailResult.Failed(exception.Message);
		}
	}
}
