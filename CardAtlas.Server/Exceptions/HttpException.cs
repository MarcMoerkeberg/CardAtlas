using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace CardAtlas.Server.Exceptions;

public class HttpException : Exception
{
	public required HttpStatusCode StatusCodeEnum { get; init; }
	public required int StatusCode { get; init; }
	public string Title { get; init; }

	[SetsRequiredMembers]
	public HttpException(HttpStatusCode statusCode, string message, string? title = null)
		: base(message)
	{
		StatusCodeEnum = statusCode;
		StatusCode = (int)statusCode;
		Title = title ?? ReasonPhrases.GetReasonPhrase(StatusCode);
	}
}
