using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Notes.Commands.CreateNote;
using Todo.Application.Notes.Commands.DeleteNote;
using Todo.Application.Notes.Commands.UpdateNote;
using Todo.Application.Notes.Queries.GetAllNotes;
using Todo.Application.Notes.Queries.GetNoteById;
using Todo.Presentation.Abstractions;
using Todo.Presentation.Constants;
using Todo.Presentation.Contracts.Notes;

namespace Todo.Presentation.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route(ApiRoutes.Notes.Base!)]
public class NotesController : ApiController
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote(
        [FromRoute] Guid todoItemId,
        [FromBody] CreateNoteRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateNoteCommand(todoItemId, request.Text);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetNoteById), new { todoItemId, id = result.Value }, result.Value);
    }

    [HttpPut(ApiRoutes.Notes.Update)]
    public async Task<IActionResult> UpdateNote(
        [FromRoute] Guid todoItemId,
        [FromRoute] Guid id,
        [FromBody] UpdateNoteRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNoteCommand(todoItemId, id, request.Text);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return NoContent();
    }

    [HttpDelete(ApiRoutes.Notes.Delete)]
    public async Task<IActionResult> DeleteNote(
        [FromRoute] Guid todoItemId,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteNoteCommand(todoItemId, id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return NoContent();
    }

    [HttpGet(ApiRoutes.Notes.GetById)]
    public async Task<IActionResult> GetNoteById(
        [FromRoute] Guid todoItemId,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetNoteByIdQuery(todoItemId, id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotes(
        [FromRoute] Guid todoItemId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllNotesQuery(todoItemId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            HandleFailure(result);
        }

        return Ok(result.Value);
    }
}