using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Domain.Shared;

namespace Todo.Presentation.Abstractions;


[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        "Bad Request",
                        "One or more errors occurred",
                        StatusCodes.Status400BadRequest,
                        result.Errors))
        };
    }

    // Questionable
    private static ProblemDetails CreateProblemDetails(
        string title,
        string type,
        string detail,
        int status,
        Error[]? errors = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Type = type,
            Detail = detail,
            Status = status
        };

        var errorDictionary = new Dictionary<string, object>();
        if (errors is not null)
        {
            errorDictionary["errors"] = errors;
        }

        // Attach the dictionary manually
        problemDetails.GetType()
            .GetProperty("Extensions")?
            .SetValue(problemDetails, errorDictionary);

        return problemDetails;
    }
}