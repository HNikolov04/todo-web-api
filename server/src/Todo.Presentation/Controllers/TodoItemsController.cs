using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Abstractions.Services;
using Todo.Application.TodoItems.Commands.CreateTodoItem;
using Todo.Application.TodoItems.Commands.DeleteTodoItem;
using Todo.Application.TodoItems.Commands.UpdateTodoItem;
using Todo.Application.TodoItems.Queries.GetAllTodoItems;
using Todo.Application.TodoItems.Queries.GetTodoItemById;
using Todo.Presentation.Abstractions;
using Todo.Presentation.Constants;
using Todo.Presentation.Contracts.TodoItems;

namespace Todo.Presentation.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route(ApiRoutes.TodoItems.Base)]
public class TodoItemsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public TodoItemsController(IMediator mediator, IUserContext userContext) : base(mediator)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        Console.WriteLine($"UserId in Controller: {userId}");

        var command = new CreateTodoItemCommand(userId, request.Title, request.DueDate);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetTodoItemById), new { id = result.Value }, result.Value);
    }

    [HttpPut(ApiRoutes.TodoItems.Update)]
    public async Task<IActionResult> UpdateTodoItem([FromRoute] Guid id, [FromBody] UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTodoItemCommand(id, request.Title, request.DueDate, request.IsStarred, request.CompletionStatus);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpDelete(ApiRoutes.TodoItems.Delete)]
    public async Task<IActionResult> DeleteTodoItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTodoItemCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpGet(ApiRoutes.TodoItems.GetById)]
    public async Task<IActionResult> GetTodoItemById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTodoItemByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoItems(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false,
        CancellationToken cancellationToken = default)
    {
        var userId = _userContext.UserId;

        var query = new GetAllTodoItemsQuery(userId, page, pageSize, search, sortBy, desc);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return Ok(result);
    }
}