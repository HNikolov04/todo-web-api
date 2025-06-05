using Todo.Application.Abstractions.CQRS;
using Todo.Domain.Errors;
using Todo.Domain.Repositories;
using Todo.Domain.Shared;

namespace Todo.Application.Notes.Queries.GetAllNotes;

public class GetAllNotesQueryHandler : IQueryHandler<GetAllNotesQuery, List<GetAllNotesResponse>>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetAllNotesQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<Result<List<GetAllNotesResponse>>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(request.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure<List<GetAllNotesResponse>>(ApplicationErrors.Todo.TodoItemNotFound);
        }

        var notes = todoItem.Notes.Select(n => new GetAllNotesResponse(n.Id, n.Text.Value)).ToList();

        return Result.Success(notes);
    }
}