using CardAtlas.Server.Resources.Errors;
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

		httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
		httpContext.Response.ContentType = "application/problem+json";

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true; //bool is returned to indicate that the exception has been handled.
	}

	private static ProblemDetails GetProblemDetails(Exception exception, HttpContext httpContext)
	{
		ProblemDetails problemDetails = exception switch
		{
			KeyNotFoundException => CreateProblemDetails(HttpStatusCode.NotFound, Errors.NotFound, exception),
			ArgumentException => CreateProblemDetails(HttpStatusCode.BadRequest, Errors.BadRequest, exception),
			UnauthorizedAccessException => CreateProblemDetails(HttpStatusCode.Unauthorized, Errors.Unauthorized, exception),
			_ => CreateProblemDetails(HttpStatusCode.InternalServerError, Errors.InternalServerError, exception),
		};

		problemDetails.Instance = httpContext.Request.Path;

		return problemDetails;
	}

	private static ProblemDetails CreateProblemDetails(HttpStatusCode status, string title, Exception exception)
	{
		return new ProblemDetails
		{
			Status = (int)status,
			Title = title,
			Detail = exception.Message,
			Type = exception.GetType().Name
		};
	}
}
