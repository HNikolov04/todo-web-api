using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;

namespace Todo.Application.Notes.Queries.GetNoteById;

public sealed class GetNoteByIdQueryHandler : IQueryHandler<GetNoteByIdQuery, GetNoteByIdResponse>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetNoteByIdQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<GetNoteByIdResponse>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure<GetNoteByIdResponse>(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var note = todoItem.Notes.FirstOrDefault(n => n.Id == request.NoteId);

        if (note is null)
        {
            return Result.Failure<GetNoteByIdResponse>(ApplicationErrors.Note.NoteNotFound);
        }

        var response = new GetNoteByIdResponse(
            note.Id,
            note.Text.Value
        );

        return Result.Success(response);
    }
}