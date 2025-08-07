namespace CardAtlas.Server.Models.Internal;

public record EmailResult
{
	public required bool Succeeded { get; init; }
	public string? ErrorMessage { get; init; }

	public static EmailResult Success => new()
	{
		Succeeded = true
	};

	public static EmailResult Failed(string error) => new()
	{
		Succeeded = false,
		ErrorMessage = error
	};
}
