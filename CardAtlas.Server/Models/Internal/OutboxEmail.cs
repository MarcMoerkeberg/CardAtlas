namespace CardAtlas.Server.Models.Internal;

public record OutboxEmail
{
	public required string ToEmail { get; init; }
	public required string Subject { get; init; }
	public required string Body { get; init; }
}
