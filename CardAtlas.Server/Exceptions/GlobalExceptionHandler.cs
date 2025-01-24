using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CardAtlas.Server.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		//TODO: Log error message with exception.
		ProblemDetails problemDetails = GetProblemDetails(exception, httpContext);

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true; //bool is returned to indicate that the exception has been handled.
	}

	private static ProblemDetails GetProblemDetails(Exception exception, HttpContext httpContext)
	{
		ProblemDetails problemDetails = exception switch
		{
			KeyNotFoundException => GetNotFoundDetails(),
			ArgumentException => GetBadRequestDetails(),
			UnauthorizedAccessException => GetUnauthorizedDetails(),
			_ => GetInternalServerErrorDetails(),
		};

		problemDetails.Detail = exception.Message;
		problemDetails.Type = exception.GetType().Name;
		problemDetails.Instance = httpContext.Request.Path;

		return problemDetails;
	}

	private static ProblemDetails GetNotFoundDetails()
	{
		return new ProblemDetails
		{
			Status = (int)HttpStatusCode.NotFound,
			Title = HttpStatusCode.NotFound.ToString(),
		};
	}

	private static ProblemDetails GetBadRequestDetails()
	{
		return new ProblemDetails
		{
			Status = (int)HttpStatusCode.BadRequest,
			Title = HttpStatusCode.BadRequest.ToString(),
		};
	}

	private static ProblemDetails GetUnauthorizedDetails()
	{
		return new ProblemDetails
		{
			Status = (int)HttpStatusCode.Unauthorized,
			Title = HttpStatusCode.Unauthorized.ToString(),
		};
	}

	private static ProblemDetails GetInternalServerErrorDetails()
	{
		return new ProblemDetails
		{
			Status = (int)HttpStatusCode.InternalServerError,
			Title = HttpStatusCode.InternalServerError.ToString(),
		};
	}
}
