namespace CardAtlas.Server.Models.Internal;

public class AppSettings
{
	public required string AppName { get; init; }
	public required ConnectionStrings ConnectionStrings { get; init; }
}

public record ConnectionStrings
{
	public required string Database { get; init; }
}
