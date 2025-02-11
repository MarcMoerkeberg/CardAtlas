using System.Net;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models;

public class Error
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

	[JsonIgnore]
	public HttpStatusCode HttpStatusCode => (HttpStatusCode)HttpStatus;
}
