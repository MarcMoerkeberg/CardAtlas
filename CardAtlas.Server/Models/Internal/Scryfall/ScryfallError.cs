using System.Net;
using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Internal.Scryfall;

public class ScryfallError
{
	[JsonPropertyName("status")]
	public int HttpStatus { get; set; }

	[JsonPropertyName("code")]
	public required string HttpCode { get; set; }

	[JsonPropertyName("details")]
	public required string ErrorDetails { get; set; }

	[JsonPropertyName("type")]
	public string? Type { get; set; }

	[JsonPropertyName("warnings")]
	public string[]? Warnings { get; set; }

	public HttpStatusCode HttpStatusCode => (HttpStatusCode)HttpStatus;
}
