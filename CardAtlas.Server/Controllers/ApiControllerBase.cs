using CardAtlas.Server.Resources.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CardAtlas.Server.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
	protected IActionResult BadRequestProblem(string detail) =>
		Problem(
			detail: detail,
			statusCode: StatusCodes.Status400BadRequest,
			title: Errors.BadRequest
		);

	protected IActionResult UnauthorizedProblem(string detail) =>
		Problem(
			detail: detail,
			statusCode: StatusCodes.Status401Unauthorized,
			title: Errors.Unauthorized
		);

	protected IActionResult ForbiddenProblem(string detail) =>
		Problem(
			detail: detail,
			statusCode: StatusCodes.Status403Forbidden,
			title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status403Forbidden)
		);

	protected IActionResult NotFoundProblem(string detail) =>
		Problem(
			detail: detail,
			statusCode: StatusCodes.Status404NotFound,
			title: Errors.NotFound
		);

	protected IActionResult ConflictProblem(string detail) =>
		Problem(
			detail: detail,
			statusCode: StatusCodes.Status409Conflict,
			title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status409Conflict)
		);
}
