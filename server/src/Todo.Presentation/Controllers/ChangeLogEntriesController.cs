using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.ChangeLogEntries.Queries.GetAllChangeLogEntries;
using Todo.Presentation.Abstractions;
using Todo.Presentation.Constants;

namespace Todo.Presentation.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route(ApiRoutes.ChangeLogEntries.Base)]
public class ChangeLogEntriesController : ApiController
{
    private readonly IMediator _mediator;

    public ChangeLogEntriesController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChangeLogEntries(
        [FromRoute] Guid todoItemId,
        CancellationToken cancellationToken)
    {
        var query = new GetAllChangeLogEntriesQuery(todoItemId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return Ok(result.Value);
    }
}