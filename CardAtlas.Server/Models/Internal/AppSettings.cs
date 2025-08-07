using System.Text;

namespace CardAtlas.Server.Models.Internal;

public class AppSettings
{
	public required string AppName { get; init; }
	public required ConnectionStrings ConnectionStrings { get; init; }
	public required JwtSettings JwtSettings { get; init; }
	public required SmtpSettings SmtpSettings { get; init; }
}

public record ConnectionStrings
{
	public required string Database { get; init; }
}

public record JwtSettings
{
	public required string Secret { get; init; }
	public byte[] Key => Encoding.UTF8.GetBytes(Secret);
	public required string Audience { get; init; }
	public required TimeSpan TimeToLive { get; init; }
}

public record SmtpSettings
{
	public required string Host { get; init; }
	public required int Port { get; init; }
	public required string Username { get; init; }
	public required string Password { get; init; }
	public required string From { get; init; }
	public required string FromName { get; init; }
}